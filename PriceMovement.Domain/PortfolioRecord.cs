namespace PriceMovement.Domain
{
    /// <summary>
    /// Portfolio record.
    /// </summary>
    public class PortfolioRecord
    {
        /// <summary>
        /// Gets or sets the portfolio code.
        /// </summary>
        public string Portfolio { get; set; }

        /// <summary>
        /// Gets or sets the portfolio id.
        /// </summary>
        public decimal PortfolioId { get; set; }

        /// <summary>
        /// Gets or sets the security description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the security id.
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
        public decimal TotalValueUsd { get; set; }

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
        public string DealingDesk { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public int RecordType { get; set; }

        /// <summary>
        /// Gets or sets the  .
        /// </summary>
        public string PriceCurrency { get; set; }
    }
}
