namespace PriceMovement.Domain
{
    /// <summary>
    /// The Serilog logging options.
    /// </summary>
    public class SerilogOptions
    {
        /// <summary>
        /// Gets or sets the log file name.
        /// </summary>
        public string LogFilename { get; set; }

        /// <summary>
        /// Gets or sets the log file location.
        /// </summary>
        public string LogFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the log file output template.
        /// </summary>
        public string OutputTemplate { get; set; }

        /// <summary>
        /// Gets or sets the email connection information object.
        /// </summary>
        public EmailConnectionInfo EmailConnectionInfo { get; set; }
    }
}