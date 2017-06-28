using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Advanced_PortChecker.Classes
{
    /// <summary>
    /// Static class to determine whether a certain port is open or not.
    /// </summary>
    internal static class PortChecker
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Determine which ports are open on a certain address using a TCP and UDP Client.
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned.</param>
        /// <param name="startPort">The starting point of ports that needs to be scanned.</param>
        /// <param name="stopPort">The final port in a range of ports that needs to be scanned.</param>
        /// <param name="timeout">The amount of time before the operation cancels.</param>
        /// <param name="oi">The operation information regarding this scan.</param>
        /// <returns>A list of information regarding the ports and address that was scanned.</returns>
        internal static async Task<List<LvCheck>> CheckTCPUDP(string address, int startPort, int stopPort, int timeout, OperationInformation oi)
        {
            List<LvCheck> lv = new List<LvCheck>();
            await Task.Run(() =>
            {
                for (int i = startPort; i <= stopPort; i++)
                {
                    if (oi.IsCancelled) return;

                    lv.AddRange(CheckTCP(address, i, i, timeout, oi, false).Result);
                    lv.AddRange(CheckUDP(address, i, i, timeout, oi, false).Result);

                    oi.Progress.Report(i);
                }
            });

            return lv;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Determine which ports are open on a certain address using a TCP Client.
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned.</param>
        /// <param name="startPort">The starting point of ports that needs to be scanned.</param>
        /// <param name="stopPort">The final port in a range of ports that needs to be scanned.</param>
        /// <param name="timeout">The amount of time before the operation cancels.</param>
        /// <param name="oi">The operation information regarding this scan.</param>
        /// <param name="reportProgress">A boolean to represent whether this method should report the current progress or not.</param>
        /// <returns>A list of information regarding the ports and address that was scanned.</returns>
        internal static async Task<List<LvCheck>> CheckTCP(string address, int startPort, int stopPort, int timeout, OperationInformation oi, bool reportProgress)
        {
            List<LvCheck> lv = new List<LvCheck>();
            await Task.Run(() =>
            {
                for (int i = startPort; i <= stopPort; i++)
                {
                    if (oi.IsCancelled) return;

                    // ReSharper disable once UseObjectOrCollectionInitializer
                    LvCheck check = new LvCheck
                    {
                        Address = address,
                        Port = i,
                        Type = "TCP",
                        Description = IsTcpOpen(address, i, timeout) ? "Open" : "Closed"
                    };
                    lv.Add(check);

                    if (reportProgress)
                    {
                        oi.Progress.Report(i);
                    }
                    oi.ItemProgress.Report(check);
                }
            });
            return lv;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Determine which ports are open on a certain address using an UDP Client.
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned.</param>
        /// <param name="startPort">The starting point of ports that needs to be scanned.</param>
        /// <param name="stopPort">The final port in a range of ports that needs to be scanned.</param>
        /// <param name="timeout">The amount of time before the operation cancels.</param>
        /// <param name="oi">The operation information regarding this scan.</param>
        /// <param name="reportProgress">A boolean to represent whether this method should report the current progress or not.</param>
        /// <returns>A list of information regarding the ports and address that was scanned.</returns>
        internal static async Task<List<LvCheck>> CheckUDP(string address, int startPort, int stopPort, int timeout, OperationInformation oi, bool reportProgress)
        {
            List<LvCheck> lv = new List<LvCheck>();
            await Task.Run(() =>
            {
                for (int i = startPort; i <= stopPort; i++)
                {
                    if (oi.IsCancelled) return;

                    // ReSharper disable once UseObjectOrCollectionInitializer
                    LvCheck check = new LvCheck
                    {
                        Address = address,
                        Port = i,
                        Type = "UDP",
                        Description = IsUdpOpen(address, i, timeout) ? "Open" : "Closed"
                    };
                    lv.Add(check);

                    if (reportProgress)
                    {
                        oi.Progress.Report(i);
                    }
                    oi.ItemProgress.Report(check);
                }
            });
            return lv;
        }


        /// <summary>
        /// Determine whether a certain port is open or not on a certain address using a TCP Client.
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned.</param>
        /// <param name="port">The port that needs to be scanned.</param>
        /// <param name="timeout">The amount of time before the operation cancels.</param>
        /// <returns>Returns true if the port is open for connections.</returns>
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
        /// Determine whether a certain port is open or not on a certain address using a UDP Client.
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned.</param>
        /// <param name="port">The port that needs to be scanned.</param>
        /// <param name="timeout">The amount of time before the operation cancels.</param>
        /// <returns>Returns true if the port is open for connections.</returns>
        private static bool IsUdpOpen(string address, int port, int timeout)
        {
            try
            {
                using (UdpClient udpClient = new UdpClient())
                {
                    udpClient.Client.ReceiveTimeout = timeout;
                    udpClient.Client.SendTimeout = timeout;

                    udpClient.Connect(address, port);
                    return true;
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return false;
        }
    }
}
