// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

use crate::result::{PortStatus, ScanResult};
use std::net::{IpAddr, Shutdown, TcpStream, ToSocketAddrs, UdpSocket};
use std::ops::Deref;
use std::sync::{Arc, Mutex};
use std::thread;
use std::time::Duration;

mod result;

fn main() {
    tauri::Builder::default()
        .invoke_handler(tauri::generate_handler![open_website, scan_port_range])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}

#[tauri::command]
fn open_website(website: &str) -> Result<(), String> {
    match open::that(website) {
        Ok(_) => Ok(()),
        Err(e) => Err(e.to_string()),
    }
}

#[tauri::command]
async fn scan_port_range(
    address: &str,
    start_port: u16,
    end_port: u16,
    timeout: u64,
    threads: u32,
    sort: bool,
) -> Result<Vec<ScanResult>, String> {
    let mut threads = threads;
    let all_results: Arc<Mutex<Vec<ScanResult>>> = Arc::new(Mutex::new(vec![]));

    if threads > 1 {
        let total_ports = u32::from(end_port) - u32::from(start_port) + 1;

        if threads > total_ports {
            threads = total_ports;
        }

        let range = (total_ports / threads) as u16;
        let remainder = (total_ports % threads) as u16;

        let mut current_start = start_port;
        let mut current_end = start_port + (range - 1);

        let mut handles = vec![];
        for n in 0..threads {
            let local_start = current_start;
            let local_end = current_end;
            let local_host = String::from(address);

            let all_results = Arc::clone(&all_results);
            let handle = thread::spawn(move || {
                let res = scan_tcp_range(&local_host, local_start, local_end, timeout);

                let mut results = all_results.lock().unwrap();
                for l in res {
                    results.push(l);
                }
            });
            handles.push(handle);

            if current_end != u16::MAX {
                current_start = current_end + 1;
            }

            match current_end.checked_add(range) {
                Some(v) => {
                    current_end = v;
                }
                None => {
                    current_end = u16::MAX;
                }
            };

            if remainder > 0 && n == threads - 2 {
                match current_end.checked_add(remainder) {
                    None => {
                        current_end = u16::MAX;
                    }
                    Some(v) => {
                        current_end = v;
                    }
                }
            }
        }

        for handle in handles {
            handle.join().unwrap();
        }
    } else {
        let res = scan_tcp_range(address, start_port, end_port, timeout);
        let mut all = all_results.lock().unwrap();
        for l in res {
            all.push(l);
        }
    }

    let mut res = all_results.lock().unwrap();

    if sort {
        // Sort by port number
        res.sort_by(|a, b| a.port.cmp(&b.port));
    }

    Ok(res.deref().to_vec())
}

/// Scan a range of ports for a specified host
///
/// # Arguments
///
/// * `host` - The host that needs to be scanned
/// * `start` - The initial port that needs to be scanned
/// * `end` - The final port that needs to be scanned
/// * `timeout` - The connection timeout (in milliseconds) before a port is marked as closed
/// * `no_closed` - Sets whether closed ports should be added to the return list or not
fn scan_tcp_range(
    host: &str,
    start: u16,
    end: u16,
    timeout: u64,
) -> Vec<ScanResult> {
    let mut scan_result = vec![];

    for n in start..=end {
        let mut address = format!("{}:{}", host, n).to_socket_addrs().unwrap();
        let socket_address = address.next().unwrap();
        let ip_addr: IpAddr = socket_address.ip();
        let host_name = ip_addr.to_string();

        if let Ok(stream) =
            TcpStream::connect_timeout(&socket_address, Duration::from_millis(timeout))
        {
            let sr = ScanResult::new(host, n, &host_name, PortStatus::Open);
            scan_result.push(sr);

            let res = stream.shutdown(Shutdown::Both);
            match res {
                Ok(_) => {}
                Err(e) => {
                    panic!("Unable to shut down TcpStream: {}", e)
                }
            }
        } else {
            let sr = ScanResult::new(host, n, &host_name, PortStatus::Closed);
            scan_result.push(sr);
        }
    }

    scan_result
}
