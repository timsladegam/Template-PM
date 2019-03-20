namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PriceMovement.Data;
    using PriceMovement.Domain;

    /// <inheritdoc cref="IPrices"/>
    public class Prices : IPrices
    {
        private readonly IPriceDataCommand priceDataCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="Prices"/> class.
        /// </summary>
        /// <param name="priceDataCommand">The price data command.</param>
        public Prices(IPriceDataCommand priceDataCommand)
        {
            this.priceDataCommand = priceDataCommand;
        }

        /// <inheritdoc cref="IPrices.GetPriceRecords"/>
        public async Task<List<PriceRecord>> GetPriceRecords(string securityId, DateTime forDate, int dayCount)
        {
            var fromDate = AddWeekdays(forDate, -dayCount);

            // Get price from data for the run date
            var task = this.GetData(securityId, fromDate, forDate);

            // wait for result
            await task.ConfigureAwait(false);

            var data = task.Result;
            return data;
        }

        private static DateTime AddWeekdays(DateTime start, int days)
        {
            var direction = Math.Sign(days);

            // if the day is a weekend, shift to the next weekday before calculating
            if ((start.DayOfWeek == DayOfWeek.Sunday && direction < 0) || (start.DayOfWeek == DayOfWeek.Saturday && direction > 0))
            {
                start = start.AddDays(direction * 2);
                days += direction * -1; // adjust days to add by one
            }
            else if ((start.DayOfWeek == DayOfWeek.Sunday && direction > 0) || (start.DayOfWeek == DayOfWeek.Saturday && direction < 0))
            {
                start = start.AddDays(direction);
                days += direction * -1; // adjust days to add by one
            }

            var weeksBase = Math.Abs(days / 5);
            var addDays = Math.Abs(days % 5);

            var totalDays = (weeksBase * 7) + addDays;
            var result = start.AddDays(totalDays * direction);

            // if the result is a weekend, shift to the next weekday
            if ((result.DayOfWeek == DayOfWeek.Sunday && direction > 0) || (result.DayOfWeek == DayOfWeek.Saturday && direction < 0))
            {
                result = result.AddDays(direction);
            }
            else if ((result.DayOfWeek == DayOfWeek.Sunday && direction < 0) || (result.DayOfWeek == DayOfWeek.Saturday && direction > 0))
            {
                result = result.AddDays(direction * 2);
            }

            return result;
        }

        private Task<List<PriceRecord>> GetData(string securityId, DateTime fromDate, DateTime forDate)
        {
            return Task.Run(() => this.priceDataCommand.GetPriceData(securityId, fromDate, forDate).ToList());
        }
    }
}
