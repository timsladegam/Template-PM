namespace PriceMovement.Data
{
    using System;
    using System.Collections.Generic;
    using PriceMovement.Domain;

    /// <summary>
    /// The portfolio data interface.
    /// </summary>
    public interface IPortfolioDataCommand
    {
        /// <summary>
        /// The get portfolio data command.
        /// </summary>
        /// <param name="runDate">The run date.</param>
        /// <returns>An enumerable of portfolio data for the run date.</returns>
        IEnumerable<PortfolioRecord> GetPortfolioData(DateTime runDate);
    }
}
