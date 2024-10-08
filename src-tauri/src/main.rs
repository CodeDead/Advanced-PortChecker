// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

use crate::result::{PortStatus, ScanResult};
use std::net::{IpAddr, Ipv4Addr, Ipv6Addr, Shutdown, TcpStream, ToSocketAddrs};
use std::ops::Deref;
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::{Arc, Mutex};
use std::thread::available_parallelism;
use std::time::Duration;
use std::{fs, thread};

mod result;

struct SharedState {
    is_scanning: Arc<AtomicBool>,
    cancellation_token: Arc<AtomicBool>,
    last_error: Arc<Mutex<String>>,
}

fn main() {
    let shared_state = SharedState {
        is_scanning: Arc::new(AtomicBool::new(false)),
        cancellation_token: Arc::new(AtomicBool::new(false)),
        last_error: Arc::new(Mutex::new(String::from(""))),
    };

    tauri::Builder::default()
        .plugin(tauri_plugin_os::init())
        .plugin(tauri_plugin_dialog::init())
        .manage(shared_state)
        .invoke_handler(tauri::generate_handler![
            open_website,
            scan_port_range,
            cancel_scan,
            get_number_of_threads,
            save_string_to_disk
        ])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}

/// Save a string to disk
///
/// # Arguments
///
/// * `content` - The content that needs to be saved
/// * `path` - The path where the content needs to be saved
///
/// # Returns
///
/// * `Ok(())` - If the content was saved successfully
/// * `Err(String)` - If the content could not be saved
#[tauri::command]
fn save_string_to_disk(content: &str, path: &str) -> Result<(), String> {
    match fs::write(path, content) {
        Ok(_) => Ok(()),
        Err(e) => Err(e.to_string()),
    }
}

/// Get the number of threads that can be used for port scanning
///
/// # Returns
///
/// * `u32` - The number of threads that can be used for port scanning
#[tauri::command]
fn get_number_of_threads() -> usize {
    let default_parallelism_approx = available_parallelism().unwrap().get();
    default_parallelism_approx
}

/// Open a website using the default browser
///
/// # Arguments
///
/// * `website` - The website that needs to be opened
///
/// # Returns
///
/// * `Ok(())` - If the website was opened successfully
/// * `Err(String)` - If the website could not be opened
#[tauri::command]
fn open_website(website: &str) -> Result<(), String> {
    match open::that(website) {
        Ok(_) => Ok(()),
        Err(e) => Err(e.to_string()),
    }
}

/// Cancel a port scan
///
/// # Arguments
///
/// * `state` - The shared state that contains the cancellation token
///
/// # Returns
///
/// * `Ok(())` - If the cancellation token was set successfully
/// * `Err(String)` - If the cancellation token could not be set
#[tauri::command]
async fn cancel_scan(state: tauri::State<'_, SharedState>) -> Result<(), String> {
    if !state.is_scanning.load(Ordering::SeqCst) {
        return Err(String::from("No scan is currently running"));
    }

    state.cancellation_token.store(true, Ordering::SeqCst);
    state.is_scanning.store(false, Ordering::SeqCst);

    Ok(())
}

/// Scan a range of ports for a specified host
///
/// # Arguments
///
/// * `state` - The shared state that contains the cancellation token
/// * `address` - The host that needs to be scanned
/// * `start_port` - The initial port that needs to be scanned
/// * `end_port` - The final port that needs to be scanned
/// * `timeout` - The connection timeout (in milliseconds) before a port is marked as closed
/// * `threads` - The number of threads that should be used to scan the ports
/// * `sort` - Whether the results should be sorted by port number
///
/// # Returns
///
/// * `Ok(Vec<ScanResult>)` - If the scan was successful
/// * `Err(String)` - If the scan was unsuccessful
#[tauri::command]
async fn scan_port_range(
    state: tauri::State<'_, SharedState>,
    address: &str,
    start_port: u16,
    end_port: u16,
    timeout: u64,
    threads: u32,
    sort: bool,
) -> Result<Vec<ScanResult>, String> {
    if state.is_scanning.load(Ordering::SeqCst) {
        return Err(String::from("A scan is already running"));
    }

    state.is_scanning.store(true, Ordering::SeqCst);
    state.last_error.lock().unwrap().clear();

    let mut address_parts = address.split('/');
    let address = match address_parts.next() {
        Some(address) => address,
        None => {
            state.is_scanning.store(false, Ordering::SeqCst);
            state.cancellation_token.store(false, Ordering::SeqCst);
            return Err(String::from("Invalid address"));
        }
    };
    let subnet = address_parts.next();

    let mut addresses: Vec<String> = vec![];
    if subnet.is_some() {
        let subnet = match subnet {
            Some(subnet) => subnet,
            None => {
                state.is_scanning.store(false, Ordering::SeqCst);
                state.cancellation_token.store(false, Ordering::SeqCst);
                return Err(String::from("Invalid subnet mask"));
            }
        };
        let subnet = match subnet.parse::<u8>() {
            Ok(subnet) => subnet,
            Err(_) => {
                state.is_scanning.store(false, Ordering::SeqCst);
                state.cancellation_token.store(false, Ordering::SeqCst);
                return Err(String::from("Invalid subnet mask"));
            }
        };

        // Validate the subnet mask
        if subnet == 0 {
            state.is_scanning.store(false, Ordering::SeqCst);
            state.cancellation_token.store(false, Ordering::SeqCst);
            return Err(String::from("Subnet mask cannot be 0"));
        }

        let ip: IpAddr = match address.parse() {
            Ok(ip) => ip,
            Err(_) => {
                state.is_scanning.store(false, Ordering::SeqCst);
                state.cancellation_token.store(false, Ordering::SeqCst);
                return Err(String::from("Invalid IP address"));
            }
        };

        match ip {
            IpAddr::V4(v4) => {
                // Validate the subnet mask
                if subnet > 32 {
                    state.is_scanning.store(false, Ordering::SeqCst);
                    state.cancellation_token.store(false, Ordering::SeqCst);
                    return Err(String::from("Invalid subnet mask"));
                }

                // Convert base IP address to a u32 integer
                let base_ip_u32 = u32::from(v4);

                // Calculate the subnet mask by shifting bits left
                let mask = !((1 << (32u8 - subnet)) - 1);

                // Get the network address by applying the subnet mask to the base IP
                let network_ip_u32 = base_ip_u32 & mask;

                // Calculate the number of host addresses in the subnet
                let num_addresses = 1 << (32 - subnet);

                for i in 0..num_addresses {
                    let cancellation_token = Arc::clone(&state.cancellation_token);
                    // Check the cancellation token and return if it's true
                    if cancellation_token.load(Ordering::Relaxed) {
                        state.is_scanning.store(false, Ordering::SeqCst);
                        state.cancellation_token.store(false, Ordering::SeqCst);
                        return Ok(vec![]);
                    }

                    let ip_u32 = network_ip_u32 + i;
                    let ip = Ipv4Addr::from(ip_u32);
                    addresses.push(ip.to_string());
                }
            }
            IpAddr::V6(v6) => {
                // Validate the subnet mask
                if subnet > 128 {
                    state.is_scanning.store(false, Ordering::SeqCst);
                    state.cancellation_token.store(false, Ordering::SeqCst);
                    return Err(String::from("Invalid subnet mask"));
                }

                // Convert the IPv6 address to a u128 representation
                let ipv6_int = u128::from_be_bytes(v6.octets());

                // Create the subnet mask
                let mask = !((1u128 << (128 - subnet)) - 1);

                // Calculate the network base address (first IP in the subnet)
                let network_base = ipv6_int & mask;

                // Calculate the range of addresses in the subnet
                let subnet_size = 1u128 << (128 - subnet);
                let last_ip = network_base + subnet_size - 1;

                // Iterate through all IP addresses in the subnet
                for ip_int in network_base..=last_ip {
                    let cancellation_token = Arc::clone(&state.cancellation_token);
                    // Check the cancellation token and return if it's true
                    if cancellation_token.load(Ordering::Relaxed) {
                        state.is_scanning.store(false, Ordering::SeqCst);
                        state.cancellation_token.store(false, Ordering::SeqCst);
                        return Ok(vec![]);
                    }
                    
                    let ip_addr = Ipv6Addr::from(ip_int.to_be_bytes());
                    addresses.push(ip_addr.to_string());
                }
            }
        }
    } else {
        addresses.push(address.to_string());
    }

    let mut threads = threads;
    let all_results: Arc<Mutex<Vec<ScanResult>>> = Arc::new(Mutex::new(vec![]));

    for address in addresses {
        let cancellation_token = Arc::clone(&state.cancellation_token);
        // Check the cancellation token and return if it's true
        if cancellation_token.load(Ordering::Relaxed) {
            state.is_scanning.store(false, Ordering::SeqCst);
            state.cancellation_token.store(false, Ordering::SeqCst);

            let res = all_results.lock().unwrap();
            return Ok(res.deref().to_vec());
        }

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

                let all_results = Arc::clone(&all_results);
                let cancellation_token = Arc::clone(&state.cancellation_token);
                let last_error = Arc::clone(&state.last_error);

                let addr_clone = address.clone();
                let handle = thread::spawn(move || {
                    let res = scan_tcp_range(
                        &addr_clone,
                        local_start,
                        local_end,
                        timeout,
                        cancellation_token,
                    );

                    let res = res.unwrap_or_else(|e| {
                        let mut last_error = last_error.lock().unwrap();
                        *last_error = e.to_string();

                        vec![]
                    });

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
            let cancellation_token = Arc::clone(&state.cancellation_token);

            let res = scan_tcp_range(&address, start_port, end_port, timeout, cancellation_token);
            let res = match res {
                Ok(res) => res,
                Err(e) => {
                    return Err(e.to_string());
                }
            };

            let mut all = all_results.lock().unwrap();
            for l in res {
                all.push(l);
            }
        }
    }

    let mut res = all_results.lock().unwrap();

    if sort {
        // Sort by port number
        res.sort_by(|a, b| a.port.cmp(&b.port));
    }

    state.is_scanning.store(false, Ordering::SeqCst);
    state.cancellation_token.store(false, Ordering::SeqCst);

    if res.is_empty() {
        let last_error = state.last_error.lock().unwrap();
        if !last_error.is_empty() {
            return Err(last_error.deref().to_string());
        }
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
/// * `cancellation_token` - A cancellation token that can be used to cancel the scan
fn scan_tcp_range(
    host: &str,
    start: u16,
    end: u16,
    timeout: u64,
    cancellation_token: Arc<AtomicBool>,
) -> Result<Vec<ScanResult>, String> {
    let mut scan_result = vec![];
    for n in start..=end {
        // Check the cancellation token and return if it's true
        if cancellation_token.load(Ordering::Relaxed) {
            return Ok(scan_result);
        }

        let address = format!("{}:{}", host, n).to_socket_addrs();
        let mut address = match address {
            Ok(res) => res,
            Err(e) => {
                return Err(e.to_string());
            }
        };

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
                    println!("Unable to shut down TcpStream: {}", e)
                }
            }
        } else {
            let sr = ScanResult::new(host, n, &host_name, PortStatus::Closed);
            scan_result.push(sr);
        }
    }

    Ok(scan_result)
}
