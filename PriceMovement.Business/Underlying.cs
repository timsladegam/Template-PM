namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PriceMovement.Data;
    using PriceMovement.Domain;

    /// <inheritdoc cref="IUnderlying"/>
    public class Underlying : IUnderlying
    {
        private readonly IUnderlyingDataCommand underlyingDataCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="Underlying"/> class.
        /// </summary>
        /// <param name="underlyingDataCommand">The underlying data command.</param>
        public Underlying(IUnderlyingDataCommand underlyingDataCommand)
        {
            this.underlyingDataCommand = underlyingDataCommand;
        }

        /// <inheritdoc cref="IUnderlying.GetUnderlyingRecords"/>
        public async Task<List<UnderlyingRecord>> GetUnderlyingRecords(DateTime runDate, string currencies = null)
        {
            var currencyList = currencies?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();

            // Get PM data for the run date
            var task = this.GetData(runDate);

            // wait for result
            await task.ConfigureAwait(false);

            // get result and filter if necessary
            var data = task.Result;

            if (currencyList.Count > 0)
            {
                data = data.Where(p => currencyList.Contains(p.PriceCurrency)).ToList();
            }

            return data;
        }

        private Task<List<UnderlyingRecord>> GetData(DateTime runDate)
        {
            return Task.Run(() => this.underlyingDataCommand.GetUnderlyingData(runDate).ToList());
        }
    }
}
