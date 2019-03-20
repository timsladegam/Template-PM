namespace PriceMovement.Data
{
    using System;
    using System.Collections.Generic;
    using PriceMovement.Domain;

    /// <summary>
    /// The portfolio data interface.
    /// </summary>
    public interface IStaticDataCommand
    {
        /// <summary>
        /// The get portfolio data command.
        /// </summary>
        /// <returns>An enumerable of portfolio data for the run date.</returns>
        LookupData GetStaticData();
    }
}
