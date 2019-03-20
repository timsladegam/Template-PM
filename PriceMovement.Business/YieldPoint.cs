namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PriceMovement.Data;
    using PriceMovement.Domain;

    /// <inheritdoc cref="IYieldPoint"/>
    public class YieldPoint : IYieldPoint
    {
        private readonly IYieldPointDataCommand yieldPointDataCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="YieldPoint"/> class.
        /// </summary>
        /// <param name="yieldPointDataCommand">The yield point data command.</param>
        public YieldPoint(IYieldPointDataCommand yieldPointDataCommand)
        {
            this.yieldPointDataCommand = yieldPointDataCommand;
        }

        /// <inheritdoc cref="IYieldPoint.GetYieldPointRecords"/>
        public async Task<List<YieldPointRecord>> GetYieldPointRecords(DateTime runDate, string assetClasses = null, string currencies = null)
        {
            var assetClassList = assetClasses?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
            var currencyList = currencies?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();

            // Get PM data for the run date
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

        private Task<List<YieldPointRecord>> GetData(DateTime runDate)
        {
            return Task.Run(() => this.yieldPointDataCommand.GetYieldPointData(runDate).ToList());
        }
    }
}
