using System;
using Advanced_PortChecker.Classes.Objects;

namespace Advanced_PortChecker.Classes.Scanner
{
    /// <summary>
    /// Represents the content of a scan operation
    /// </summary>
    internal sealed class ScanOperation
    {
        /// <summary>
        /// A boolean to indicate whether an operation was cancelled
        /// </summary>
        internal bool IsCancelled { get; set; }
        /// <summary>
        /// A integer value indicating the current progress
        /// </summary>
        internal IProgress<int> Progress { get; set; }
        /// <summary>
        /// The LvCheck item that is currently undergoing an operation
        /// </summary>
        internal IProgress<LvCheck> ItemProgress { get; set; }
        /// <summary>
        /// Delegate that can be called when the ScanOperation has completed its work
        /// </summary>
        internal delegate void ScanOperationCompleted();
        /// <summary>
        /// Event that can be used to indicate that a ScanOperation has completed its work
        /// </summary>
        internal ScanOperationCompleted ScanCompletedEvent;
    }
}
