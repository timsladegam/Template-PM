namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PriceMovement.Data;
    using PriceMovement.Domain;

    /// <inheritdoc cref="IStaleYield"/>
    public class StaleYield : IStaleYield
    {
        private readonly IStaleYieldDataCommand staleYieldDataCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaleYield"/> class.
        /// </summary>
        /// <param name="staleYieldDataCommand">The stale yield data command.</param>
        public StaleYield(IStaleYieldDataCommand staleYieldDataCommand)
        {
            this.staleYieldDataCommand = staleYieldDataCommand;
        }

        /// <inheritdoc cref="IStaleYield.GetStaleYieldRecords"/>
        public async Task<List<StaleYieldRecord>> GetStaleYieldRecords(DateTime runDate, string assetClasses = null, string currencies = null)
        {
            var assetClassList = assetClasses?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
            var currencyList = currencies?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();

            // Get task to return stale yield data for the run date
            var task = this.GetData(runDate);

            // wait for result
            await task.ConfigureAwait(false);

            // get result and filter if necessary
            var data = task.Result;

            if (assetClassList.Count > 0)
            {
                data = data.Where(p => assetClassList.Contains(p.AssetClass)).ToList();
            }

            if (currencyList.Count > 0)
            {
                data = data.Where(p => currencyList.Contains(p.PriceCurrency)).ToList();
            }

            return data;
        }

        private Task<List<StaleYieldRecord>> GetData(DateTime runDate)
        {
            return Task.Run(() => this.staleYieldDataCommand.GetStaleYieldData(runDate).ToList());
        }
    }
}