namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PriceMovement.Domain;

    /// <summary>
    /// The stale yield.
    /// </summary>
    public interface IStaleYield
    {
        /// <summary>
        /// Get the stale yield data.
        /// </summary>
        /// <param name="runDate">The run date.</param>
        /// <param name="assetClasses">The asset classes.</param>
        /// <param name="currencies">The currencies.</param>
        /// <returns>The stale yield records.</returns>
        Task<List<StaleYieldRecord>> GetStaleYieldRecords(DateTime runDate, string assetClasses = null, string currencies = null);
    }
}
