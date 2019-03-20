namespace PriceMovement.Domain
{
    using System;

    /// <summary>
    /// The underlying record.
    /// </summary>
    public class UnderlyingRecord
    {
        /// <summary>
        /// Gets or sets the Portfolio.
        /// </summary>
        public string Portfolio { get; set; }

        /// <summary>
        /// Gets or sets the PortfolioId.
        /// </summary>
        public decimal PortfolioId { get; set; }

        /// <summary>
        /// Gets or sets the PortfolioNavCustom.
        /// </summary>
        public decimal PortfolioNavCustom { get; set; }

        /// <summary>
        /// Gets or sets the SecurityId.
        /// </summary>
        public string SecurityId { get; set; }

        /// <summary>
        /// Gets or sets the LinkedSecurity.
        /// </summary>
        public string LinkedSecurity { get; set; }

        /// <summary>
        /// Gets or sets the Price.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the  PriceDate.
        /// </summary>
        public DateTime? PriceDate { get; set; }

        /// <summary>
        /// Gets or sets the PriceCurrency.
        /// </summary>
        public string PriceCurrency { get; set; }

        /// <summary>
        /// Gets or sets the BusinessDaysSincePriceDate.
        /// </summary>
        public decimal BusinessDaysSincePriceDate { get; set; }
    }
}
