use chrono::{DateTime, Utc};
use serde::{Deserialize, Serialize};
use std::time::SystemTime;

#[derive(Serialize, Deserialize, Clone)]
pub enum PortStatus {
    Open,
    Closed,
}

#[derive(Serialize, Deserialize, Clone)]
pub struct ScanResult {
    pub address: String,
    pub port: u16,
    #[serde(rename = "hostName")]
    pub host_name: String,
    #[serde(rename = "portStatus")]
    pub port_status: PortStatus,
    #[serde(rename = "scanDate")]
    pub scan_date: String,
}

impl ScanResult {
    /// Create a new ScanResult
    ///
    /// # Arguments
    ///
    /// * `address` - The address that was scanned
    /// * `port` - The port that was scanned
    /// * `host_name` - The host name of the address that was scanned
    /// * `port_status` - The status of the port that was scanned
    ///
    /// # Returns
    ///
    /// A new ScanResult
    pub fn new(address: &str, port: u16, host_name: &str, port_status: PortStatus) -> ScanResult {
        let now = SystemTime::now();
        let now: DateTime<Utc> = now.into();
        let now = now.to_rfc3339();

        ScanResult {
            address: String::from(address),
            port,
            host_name: String::from(host_name),
            port_status,
            scan_date: now,
        }
    }
}
