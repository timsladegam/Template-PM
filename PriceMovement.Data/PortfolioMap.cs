namespace PriceMovement.Data
{
    using Dapper.FluentMap.Mapping;

    using PriceMovement.Domain;

    /// <summary>
    /// The map.
    /// </summary>
    public class PortfolioMap : EntityMap<PortfolioRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioMap"/> class.
        /// </summary>
        public PortfolioMap()
        {
            this.Map(i => i.Portfolio).ToColumn("Portfolio");
            this.Map(i => i.PortfolioId).ToColumn("Portfolio Id");
            this.Map(i => i.SecurityId).ToColumn("Security Id");
            this.Map(i => i.Description).ToColumn("Description");
            this.Map(i => i.AssetClass).ToColumn("Asset Class");
            this.Map(i => i.AssetSubClass).ToColumn("Asset Sub-Class");
            this.Map(i => i.AssetType).ToColumn("Asset Type");
            this.Map(i => i.AssetSubtype).ToColumn("Asset Sub-type");
            this.Map(i => i.Nominal).ToColumn("Nominal");
            this.Map(i => i.TotalValueUsd).ToColumn("Total Value (USD)");
            this.Map(i => i.TotalValueUsd1dActual).ToColumn("Total Value (USD) -1d Actual");
            this.Map(i => i.TotalValueUsdChange).ToColumn("Total Value (USD) Change %");
            this.Map(i => i.PortfolioNavCustom).ToColumn("Portfolio NAV (Custom)");
            this.Map(i => i.PortfolioNav1dCustom).ToColumn("Portfolio NAV (Custom)(T-1)");
            this.Map(i => i.PortfolioNav).ToColumn("Portfolio NAV");
            this.Map(i => i.PortfolioNav1d).ToColumn("Portfolio NAV -1d");
            this.Map(i => i.ShareOfNav).ToColumn("Share of NAV");
            this.Map(i => i.DealingDesk).ToColumn("Dealing Desk");
            this.Map(i => i.RecordType).ToColumn("Report Type");
            this.Map(i => i.PriceCurrency).ToColumn("Price Currency");
            this.Map(i => i.LegType).ToColumn("Leg Type");
        }
    }
}
