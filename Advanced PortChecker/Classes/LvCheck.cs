namespace Advanced_PortChecker.Classes
{
    /// <summary>
    /// Represents the content of a ListView item
    /// </summary>
    public class LvCheck
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
