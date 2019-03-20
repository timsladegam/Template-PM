namespace PriceMovement.Data
{
    using Dapper.FluentMap.Mapping;

    using PriceMovement.Domain;

    /// <summary>
    /// The map.
    /// </summary>
    public class PriceMap : EntityMap<PriceRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PriceMap"/> class.
        /// </summary>
        public PriceMap()
        {
            this.Map(i => i.Price).ToColumn("dfMidPrice");
            this.Map(i => i.PriceCurrency).ToColumn("sPriceCurrency");
            this.Map(i => i.SecurityId).ToColumn("sSecurityId");
            this.Map(i => i.Source).ToColumn("sSource");
            this.Map(i => i.PricePoint).ToColumn("dtPricePoint");
            this.Map(i => i.ExternalFactor).ToColumn("dfExternalPriceFactor");
        }
    }
}
