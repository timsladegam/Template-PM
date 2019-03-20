namespace PriceMovement.Data
{
    using System;
    using System.Collections.Generic;
    using PriceMovement.Domain;

    /// <summary>
    /// The yield point data interface.
    /// </summary>
    public interface IYieldPointDataCommand
    {
        /// <summary>
        /// The get yield point data command.
        /// </summary>
        /// <param name="runDate">The run date.</param>
        /// <returns>An enumerable of yield point data for the run date.</returns>
        IEnumerable<YieldPointRecord> GetYieldPointData(DateTime runDate);
    }
}
