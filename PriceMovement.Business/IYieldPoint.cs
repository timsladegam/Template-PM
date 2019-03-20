namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PriceMovement.Domain;

    /// <summary>
    /// The yield point interface.
    /// </summary>
    public interface IYieldPoint
    {
        /// <summary>
        /// Get yield point data.
        /// </summary>
        /// <param name="runDate">The run date.</param>
        /// <param name="assetClasses">The asset classes.</param>
        /// <param name="currencies">The currencies.</param>
        /// <returns>The yield point records..</returns>
        Task<List<YieldPointRecord>> GetYieldPointRecords(DateTime runDate, string assetClasses = null, string currencies = null);
    }
}
