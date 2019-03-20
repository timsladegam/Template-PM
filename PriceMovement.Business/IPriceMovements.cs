namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PriceMovement.Domain;

    /// <summary>
    /// Price movements interface.
    /// </summary>
    public interface IPriceMovements
    {
        /// <summary>
        /// Gets the price movement records.
        /// </summary>
        /// <param name="runDate">The run date.</param>
        /// <param name="assetClasses">The asset class list.</param>
        /// <param name="currencies">The currency list.</param>
        /// <param name="dealingDesks">The dealing desk list.</param>
        /// <param name="recordType">The record type.</param>
        /// <returns>A task.</returns>
        Task<List<PriceMovementRecord>> GetPriceMovementRecords(DateTime runDate, string assetClasses = null, string currencies = null, string dealingDesks = null, string recordType = null);

        /// <summary>
        /// Gets the lookup data.
        /// </summary>
        /// <returns>Returns lookup data.</returns>
        Task<LookupData> GetLookupData();

        /// <summary>
        /// Reset any cached data.
        /// </summary>
        void Reset();
    }
}
