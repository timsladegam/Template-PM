namespace PriceMovement.Data
{
    using System;
    using System.Collections.Generic;

    using PriceMovement.Domain;

    /// <summary>
    /// The price data command interface.
    /// </summary>
    public interface IPriceDataCommand
    {
        /// <summary>
        /// Get prices for date for list of security ids.
        /// </summary>
        /// <param name="runDate">The run date.</param>
        /// <returns>The price data.</returns>
        IEnumerable<PriceRecord> GetPriceData(DateTime runDate);

        /// <summary>
        /// Get all prices for a security id between a from and to date.
        /// </summary>
        /// <param name="securityId">The security id.</param>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <returns>The price data.</returns>
        IEnumerable<PriceRecord> GetPriceData(string securityId, DateTime fromDate, DateTime toDate);
    }
}
