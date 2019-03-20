namespace PriceMovement.Domain
{
    using System;

    /// <summary>
    /// Dat sent to to server from client.
    /// </summary>
    public class PostData
    {
        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        public DateTime? ForDate { get; set; }

        /// <summary>
        /// Gets or sets the DayCount.
        /// </summary>
        public int DayCount { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string AssetClasses { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string Currencies { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string DealingDesks { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string RecordType { get; set; }
    }
}
