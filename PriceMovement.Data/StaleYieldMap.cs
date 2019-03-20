namespace PriceMovement.Data
{
    using Dapper.FluentMap.Mapping;

    using PriceMovement.Domain;

    /// <summary>
    /// Map data to entity.
    /// </summary>
    public class StaleYieldMap : EntityMap<StaleYieldRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaleYieldMap"/> class.
        /// </summary>
        public StaleYieldMap()
        {
            this.Map(i => i.SecurityId).ToColumn("Identifier");
            this.Map(i => i.Description).ToColumn("Description");
            this.Map(i => i.PriceCurrency).ToColumn("Price Currency");
            this.Map(i => i.Price).ToColumn("Current Price");
            this.Map(i => i.PriceDate).ToColumn("Last Price Date");
            this.Map(i => i.Price1d).ToColumn("Price -1d (1 days ago)");
            this.Map(i => i.PriceDate1d).ToColumn(string.Empty);
            this.Map(i => i.Factor).ToColumn("Latest Factor");
            this.Map(i => i.Factor1d).ToColumn("Factor -1d (1 days ago)");
            this.Map(i => i.AssetClass).ToColumn("Asset Class");
            this.Map(i => i.AssetSubClass).ToColumn("Asset Sub-class");
            this.Map(i => i.BusinessDaysSincePriceDate).ToColumn("Stale");
        }
    }
}
