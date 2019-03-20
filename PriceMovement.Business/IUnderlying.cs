namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PriceMovement.Domain;

    /// <summary>
    /// The underlying interface.
    /// </summary>
    public interface IUnderlying
    {
        /// <summary>
        /// Get the underlying records.
        /// </summary>
        /// <param name="runDate">The run date.</param>
        /// <param name="currencies">The currencies.</param>
        /// <returns>The underlying records.</returns>
        Task<List<UnderlyingRecord>> GetUnderlyingRecords(DateTime runDate, string currencies = null);
    }
}
