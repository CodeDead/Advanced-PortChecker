using System;
using System.Net.Sockets;

namespace Advanced_PortChecker.Classes
{
    /// <summary>
    /// Determine whether a certain port is open or not.
    /// </summary>
    internal static class PortChecker
    {
        /// <summary>
        /// Determine whether a certain port is open or not on a certain address using a TCP Client.
        /// </summary>
        /// <param name="address">The IP address that needs to be scanned.</param>
        /// <param name="port">The port that needs to be scanned.</param>
        /// <returns>Returns true if the port is open for connections.</returns>
        internal static bool IsTcpOpen(string address, int port)
        {
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect(address, port);
                }
                return true;
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
        /// <returns>Returns true if the port is open for connections.</returns>
        internal static bool IsUdpOpen(string address, int port)
        {
            try
            {
                using (UdpClient udpClient = new UdpClient())
                {
                    udpClient.Connect(address, port);
                }
                return true;
            }
            catch (Exception)
            {
                // ignored
            }
            return false;
        }
    }
}
