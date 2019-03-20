namespace PriceMovement.Domain
{
    /// <summary>
    /// Email connection information.
    /// </summary>
    public class EmailConnectionInfo
    {
        /// <summary>
        /// Gets or sets the email server.
        /// </summary>
        public string MailServer { get; set; }

        /// <summary>
        /// Gets or sets the email server port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets a default email subject.
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets the email sender.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the email recipients.
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of events to post in a single batch (default is 100).
        /// </summary>
        public int? BatchPostingLimit { get; set; }
    }
}
