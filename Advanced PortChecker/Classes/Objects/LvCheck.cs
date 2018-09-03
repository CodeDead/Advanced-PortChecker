namespace Advanced_PortChecker.Classes.Objects
{
    /// <summary>
    /// Represents the content of a ListViewItem
    /// </summary>
    public class LvCheck
    {
        /// <summary>
        /// The address that was scanned
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// The port that was scanned
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// The hostname of the machine that was scanned
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// The type of scan that was performed
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Description of the scan result
        /// </summary>
        public string Description { get; set; }
    }
}
