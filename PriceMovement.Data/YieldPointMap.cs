namespace PriceMovement.Data
{
    using Dapper.FluentMap.Mapping;

    using PriceMovement.Domain;

    /// <summary>
    /// The map.
    /// </summary>
    public class YieldPointMap : EntityMap<YieldPointRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YieldPointMap"/> class.
        /// </summary>
        public YieldPointMap()
        {
            this.Map(i => i.PriceChange).ToColumn("Price Change");
            this.Map(i => i.SecurityId).ToColumn("Identifier");
            this.Map(i => i.Description).ToColumn("Description");
            this.Map(i => i.PriceCurrency).ToColumn("price currency");
            this.Map(i => i.DisplayFactor).ToColumn("price display factor");
            this.Map(i => i.Price).ToColumn("Current Price");
            this.Map(i => i.PriceDate).ToColumn("Last Price Date");
            this.Map(i => i.Price1d).ToColumn("Price -1d (1 days ago)");
            this.Map(i => i.Factor).ToColumn("Latest Factor");
            this.Map(i => i.Factor1d).ToColumn("Factor -1d (1 days ago)");
            this.Map(i => i.AssetClass).ToColumn("Asset Class");
            this.Map(i => i.AssetSubClass).ToColumn("Asset Sub-class");
        }
    }
}
