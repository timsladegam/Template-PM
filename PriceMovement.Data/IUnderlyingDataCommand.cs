namespace PriceMovement.Data
{
    using System;
    using System.Collections.Generic;
    using PriceMovement.Domain;

    /// <summary>
    /// The underlying data interface.
    /// </summary>
    public interface IUnderlyingDataCommand
    {
        /// <summary>
        /// The get underlying data command.
        /// </summary>
        /// <param name="runDate">The run date.</param>
        /// <returns>An enumerable of underlying data for the run date.</returns>
        IEnumerable<UnderlyingRecord> GetUnderlyingData(DateTime runDate);
    }
}
