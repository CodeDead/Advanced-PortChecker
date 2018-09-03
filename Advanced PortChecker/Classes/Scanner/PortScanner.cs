using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Advanced_PortChecker.Classes.Objects;

namespace Advanced_PortChecker.Classes.Scanner
{
    /// <summary>
    /// Static class to determine whether a certain port is open or not
    /// </summary>
    internal static class PortScanner
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Determine which ports are open on a certain address using a TCP and UDP Client
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned</param>
        /// <param name="startPort">The starting point of ports that needs to be scanned</param>
        /// <param name="stopPort">The final port in a range of ports that needs to be scanned</param>
        /// <param name="timeout">The amount of time before a connection times out</param>
        /// <param name="scanOperation">The ScanInformation object containing information regarding this scan</param>
        /// <returns>A list of LvCheck objects containing information regarding the ports and address that were scanned</returns>
        // ReSharper disable once IdentifierTypo
        internal static List<LvCheck> CheckTCPUDP(string address, int startPort, int stopPort, int timeout, ScanOperation scanOperation)
        {
            List<LvCheck> lv = new List<LvCheck>();
            for (int i = startPort; i <= stopPort; i++)
            {
                if (scanOperation.IsCancelled) break;

                lv.AddRange(CheckTCP(address, i, i, timeout, scanOperation, false));
                lv.AddRange(CheckUDP(address, i, i, timeout, scanOperation, false));

                scanOperation.Progress.Report(1);
            }
            scanOperation.ScanCompletedEvent?.Invoke();
            return lv;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Determine which ports are open on a certain address using a TCP Client
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned</param>
        /// <param name="startPort">The starting point of ports that needs to be scanned</param>
        /// <param name="stopPort">The final port in a range of ports that needs to be scanned</param>
        /// <param name="timeout">The amount of time before the connection times out</param>
        /// <param name="scanOperation">The ScanInformation object containing information regarding this scan</param>
        /// <param name="reportProgress">A boolean to represent whether this method should report the current progress or not</param>
        /// <returns>A list of LvCheck objects containing information regarding the ports and address that were scanned</returns>
        internal static List<LvCheck> CheckTCP(string address, int startPort, int stopPort, int timeout, ScanOperation scanOperation, bool reportProgress)
        {
            List<LvCheck> lv = new List<LvCheck>();
            for (int i = startPort; i <= stopPort; i++)
            {
                if (scanOperation.IsCancelled) break;

                LvCheck check = new LvCheck
                {
                    Address = address,
                    Port = i,
                    HostName = GetMachineNameFromIpAddress(address),
                    Type = "TCP",
                    Description = IsTcpOpen(address, i, timeout) ? "Open" : "Closed",
                    ScanDate = DateTime.Now.ToString(CultureInfo.CurrentCulture)
                };
                lv.Add(check);

                if (reportProgress)
                {
                    scanOperation.Progress.Report(1);
                }
                scanOperation.ItemProgress.Report(check);
            }

            if (reportProgress)
            {
                scanOperation.ScanCompletedEvent?.Invoke();
            }
            
            return lv;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Determine which ports are open on a certain address using an UDP Client
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned</param>
        /// <param name="startPort">The starting point of ports that needs to be scanned</param>
        /// <param name="stopPort">The final port in a range of ports that needs to be scanned</param>
        /// <param name="timeout">The amount of time before the connection times out</param>
        /// <param name="scanOperation">The ScanInformation object containing information regarding this scan</param>
        /// <param name="reportProgress">A boolean to represent whether this method should report the current progress or not</param>
        /// <returns>A list of LvCheck objects containing information regarding the ports and address that were scanned</returns>
        internal static List<LvCheck> CheckUDP(string address, int startPort, int stopPort, int timeout, ScanOperation scanOperation, bool reportProgress)
        {
            List<LvCheck> lv = new List<LvCheck>();
            for (int i = startPort; i <= stopPort; i++)
            {
                if (scanOperation.IsCancelled) break;

                LvCheck check = new LvCheck
                {
                    Address = address,
                    Port = i,
                    HostName = GetMachineNameFromIpAddress(address),
                    Type = "UDP",
                    Description = IsUdpOpen(address, i, timeout) ? "Open" : "Closed",
                    ScanDate = DateTime.Now.ToString(CultureInfo.CurrentCulture)
                };
                lv.Add(check);

                if (reportProgress)
                {
                    scanOperation.Progress.Report(1);
                }
                scanOperation.ItemProgress.Report(check);
            }

            if (reportProgress)
            {
                scanOperation.ScanCompletedEvent?.Invoke();
            }
            return lv;
        }


        /// <summary>
        /// Determine whether a certain port is open or not on a certain address using a TCP Client
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned</param>
        /// <param name="port">The port that needs to be scanned</param>
        /// <param name="timeout">The amount of time before the connection times out</param>
        /// <returns>Returns true if the port is open</returns>
        private static bool IsTcpOpen(string address, int port, int timeout)
        {
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.SendTimeout = timeout;
                    tcpClient.ReceiveTimeout = timeout;

                    return tcpClient.ConnectAsync(address, port).Wait(timeout);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return false;
        }

        /// <summary>
        /// Determine whether a certain port is open or not on a certain address using a UDP Client
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned</param>
        /// <param name="port">The port that needs to be scanned</param>
        /// <param name="timeout">The amount of time before the connection times out</param>
        /// <returns>Returns true if the port is open</returns>
        private static bool IsUdpOpen(string address, int port, int timeout)
        {
            try
            {
                using (UdpClient udpClient = new UdpClient())
                {
                    udpClient.Client.ReceiveTimeout = timeout;
                    udpClient.Client.SendTimeout = timeout;

                    udpClient.Connect(address, port);

                    byte[] sendBytes = new byte[4];
                    new Random().NextBytes(sendBytes);
                    udpClient.Send(sendBytes, sendBytes.Length);
                    
                    IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                    byte[] result = udpClient.Receive(ref remoteIpEndPoint);
                    return result.Length != 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the Host name by IP address
        /// </summary>
        /// <param name="ipAddress">The IP address that needs to be resolved to a host name</param>
        /// <returns>The host name of an IP address</returns>
        private static string GetMachineNameFromIpAddress(string ipAddress)
        {
            string machineName = string.Empty;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                machineName = hostEntry.HostName;
            }
            catch (Exception)
            {
                // Machine not found...
            }
            return machineName;
        }
    }
}
