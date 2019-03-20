namespace PriceMovement.Domain
{
    using System;
    using System.Dynamic;

    /// <summary>
    /// The stale yield record.
    /// </summary>
    public class StaleYieldRecord
    {
        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string SecurityId { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string PriceCurrency { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public DateTime? PriceDate { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal? Price1d { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public DateTime? PriceDate1d { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal Factor { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal Factor1d { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string AssetClass { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string AssetSubClass { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string AssetType { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string AssetSubtype { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal BusinessDaysSincePriceDate { get; set; }
    }
}
