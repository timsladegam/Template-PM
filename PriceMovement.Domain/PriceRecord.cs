namespace PriceMovement.Domain
{
    using System;

    /// <summary>
    /// The price record.
    /// </summary>
    public class PriceRecord
    {
        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string SecurityId { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string PriceCurrency { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public DateTime PricePoint { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public double ExternalFactor { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public int RowNum { get; set; }
    }
}
