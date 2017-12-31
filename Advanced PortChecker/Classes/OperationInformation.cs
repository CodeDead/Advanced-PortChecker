using System;

namespace Advanced_PortChecker.Classes
{
    /// <summary>
    /// Represents the content of a scan operation
    /// </summary>
    internal sealed class OperationInformation
    {
        /// <summary>
        /// A boolean to indicate whether an operation was cancelled
        /// </summary>
        public bool IsCancelled { get; set; }
        /// <summary>
        /// A integer value indicating the current progress
        /// </summary>
        public IProgress<int> Progress { get; set; }
        /// <summary>
        /// The LvCheck item that is currently udergoing an operation
        /// </summary>
        public IProgress<LvCheck> ItemProgress { get; set; }
    }
}
