namespace PriceMovement.Data
{
    using System;
    using System.Collections.Generic;
    using PriceMovement.Domain;

    /// <summary>
    /// The stale yield data interface.
    /// </summary>
    public interface IStaleYieldDataCommand
    {
        /// <summary>
        /// The get stale yield data command.
        /// </summary>
        /// <param name="runDate">The run date.</param>
        /// <returns>An enumerable of stale yield data for the run date.</returns>
        IEnumerable<StaleYieldRecord> GetStaleYieldData(DateTime runDate);
    }
}
