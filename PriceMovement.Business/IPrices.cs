namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PriceMovement.Domain;

    /// <summary>
    /// The prices interface.
    /// </summary>
    public interface IPrices
    {
        /// <summary>
        /// Get teh price records.
        /// </summary>
        /// <param name="securityId">The security id.</param>
        /// <param name="forDate">The for date.</param>
        /// <param name="dayCount">The day count.</param>
        /// <returns>The price records.</returns>
        Task<List<PriceRecord>> GetPriceRecords(string securityId, DateTime forDate, int dayCount);
    }
}
