namespace PriceMovement.Domain
{
    using System;

    /// <summary>
    /// The yield point record.
    /// </summary>
    public class YieldPointRecord
    {
        /// <summary>
        /// Gets or sets the  PriceChange.
        /// </summary>
        public decimal? PriceChange { get; set; }

        /// <summary>
        /// Gets or sets the SecurityId.
        /// </summary>
        public string SecurityId { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the PriceCurrency.
        /// </summary>
        public string PriceCurrency { get; set; }

        /// <summary>
        /// Gets or sets the DisplayFactor.
        /// </summary>
        public decimal DisplayFactor { get; set; }

        /// <summary>
        /// Gets or sets the Price.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the PriceDate.
        /// </summary>
        public DateTime? PriceDate { get; set; }

        /// <summary>
        /// Gets or sets the Price1d.
        /// </summary>
        public decimal? Price1d { get; set; }

        /// <summary>
        /// Gets or sets the PriceDate1d.
        /// </summary>
        public DateTime? PriceDate1d { get; set; }

        /// <summary>
        /// Gets or sets the Factor.
        /// </summary>
        public decimal Factor { get; set; }

        /// <summary>
        /// Gets or sets the Factor1d.
        /// </summary>
        public decimal Factor1d { get; set; }

        /// <summary>
        /// Gets or sets the AssetClass.
        /// </summary>
        public string AssetClass { get; set; }

        /// <summary>
        /// Gets or sets the AssetSubClass.
        /// </summary>
        public string AssetSubClass { get; set; }

        /// <summary>
        /// Gets or sets the AssetType.
        /// </summary>
        public string AssetType { get; set; }

        /// <summary>
        /// Gets or sets the AssetSubtype.
        /// </summary>
        public string AssetSubtype { get; set; }

        /// <summary>
        /// Gets or sets the BusinessDaysSincePriceDate.
        /// </summary>
        public decimal BusinessDaysSincePriceDate { get; set; }
    }
}
