namespace PriceMovement.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Dat sent to to server from client.
    /// </summary>
    public class LookupData
    {
        /// <summary>
        /// Gets the currency lookup data.
        /// </summary>
        public List<string> Currencies { get; } = new List<string>();

        /// <summary>
        /// Gets the dealing desk lookup data.
        /// </summary>
        public List<string> DealingDesks { get; } = new List<string>();

        /// <summary>
        /// Gets the asset classes lookup data.
        /// </summary>
        public List<string> AssetClasses { get; } = new List<string>();
    }
}
