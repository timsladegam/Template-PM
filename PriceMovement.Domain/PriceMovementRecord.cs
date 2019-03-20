namespace PriceMovement.Domain
{
    using System;

    /// <summary>
    /// The price movement record.
    /// </summary>
    public class PriceMovementRecord
    {
        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string Portfolio { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal PortfolioId { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string SecurityId { get; set; }

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
        /// Gets or sets the leg type.
        /// </summary>
        public string LegType { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal Nominal { get; set; }

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
        public string PriceSource { get; set; }

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
        public string PriceSource1d { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal BusinessDaysSincePriceDate { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal? PriceChange { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal? Proportion { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal TotalValueUsd { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal? TotalValueUsd1d { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal TotalValueUsd1dActual { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal TotalValueUsdChange { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal PortfolioNav { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal PortfolioNav1d { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal PortfolioNavCustom { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal PortfolioNav1dCustom { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal ShareOfNav { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal? ShareOfNav1d { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal? ShareOfNavChange { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public decimal? MarketValueImpact { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string DealingDesk { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string RecordType { get; set; }
    }
}
