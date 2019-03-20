namespace PriceMovement.Data
{
    using Dapper.FluentMap.Mapping;

    using PriceMovement.Domain;

    /// <summary>
    /// The map.
    /// </summary>
    public class UnderlyingMap : EntityMap<UnderlyingRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnderlyingMap"/> class.
        /// </summary>
        public UnderlyingMap()
        {
            this.Map(i => i.Portfolio).ToColumn("Portfolio");
            this.Map(i => i.SecurityId).ToColumn("Security Id");
            this.Map(i => i.PortfolioNavCustom).ToColumn("Portfolio NAV (Custom)");
            this.Map(i => i.LinkedSecurity).ToColumn("Linked Security");
            this.Map(i => i.Price).ToColumn("[CurrentPrice]");
            this.Map(i => i.PriceDate).ToColumn("Price Date");
            this.Map(i => i.PriceCurrency).ToColumn("Price Currency");
            this.Map(i => i.BusinessDaysSincePriceDate).ToColumn("Stale");
        }
    }
}
