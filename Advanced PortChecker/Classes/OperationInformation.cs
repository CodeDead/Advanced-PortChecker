using System;

namespace Advanced_PortChecker.Classes
{
    /// <summary>
    /// Represents the content of a scan operation.
    /// </summary>
    internal class OperationInformation
    {
        public bool IsCancelled { get; set; }

        public IProgress<int> Progress { get; set; }

        public IProgress<LvCheck> Preview { get; set; }
    }
}
