namespace PriceMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;

    using PriceMovement.Data;
    using PriceMovement.Domain;
    using PriceMovement.Infrastructure.Logging;

    /// <summary>
    /// <inheritdoc cref="IPriceMovements"/>
    /// </summary>
    public class PriceMovements : IPriceMovements
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();

        private readonly IPortfolioDataCommand portfolioDataCommand;
        private readonly IPriceDataCommand priceDataCommand;
        private readonly IStaticDataCommand staticDataCommand;
        private readonly IMemoryCache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceMovements"/> class.
        /// </summary>
        /// <param name="portfolioDataCommand">The portfolio data command.</param>
        /// <param name="priceDataCommand">The price data command.</param>
        /// <param name="staticDataCommand">The static data command.</param>
        /// <param name="memoryCache">the memory cahce.</param>
        public PriceMovements(IPortfolioDataCommand portfolioDataCommand, IPriceDataCommand priceDataCommand, IStaticDataCommand staticDataCommand, IMemoryCache memoryCache)
        {
            this.portfolioDataCommand = portfolioDataCommand;
            this.priceDataCommand = priceDataCommand;
            this.staticDataCommand = staticDataCommand;
            this.cache = memoryCache;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static ILogger Logger => AppLogging.CreateLogger<PortfolioDataCommand>();

        /// <inheritdoc cref="IPriceMovements.GetPriceMovementRecords"/>
        public async Task<List<PriceMovementRecord>> GetPriceMovementRecords(DateTime runDate, string assetClasses = null, string currencies = null, string dealingDesks = null, string recordType = null)
        {
            var assetClassList = assetClasses?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
            var currencyList = currencies?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
            var dealingDeskList = dealingDesks?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();

            // We need price data for the price date and previous price date
            var priceDate = AddWeekdays(runDate, -1);

            // Get Portfolio and price data for the run date
            var portfolioDataTask = this.GetPortolioData(runDate);
            var pricesTask = this.GetPriceData(priceDate);

            // wait till data is available
            await Task.WhenAll(portfolioDataTask, pricesTask).ConfigureAwait(false);

            // filter portfolio data
            var portfolioData = portfolioDataTask.Result;
            if (!string.IsNullOrEmpty(recordType))
            {
                var recordDigits = recordType == "ETD" ? new List<int> { 1, 2 } : (recordType == "OTC" ? new List<int> { 3 } : new List<int> { 4 });
                portfolioData = portfolioData.Where(p => recordDigits.Contains(p.RecordType)).ToList();
            }

            if (assetClassList.Count > 0)
            {
                portfolioData = portfolioData.Where(p => assetClassList.Contains(p.AssetClass)).ToList();
            }

            if (currencyList.Count > 0)
            {
                portfolioData = portfolioData.Where(p => currencyList.Contains(p.PriceCurrency)).ToList();
            }

            if (dealingDeskList.Count > 0)
            {
                portfolioData = portfolioData.Where(p => dealingDeskList.Contains(p.DealingDesk)).ToList();
            }

            Logger.LogInformation(default(EventId), message: "Portfolio data filtered.");

            // filter price data
            var prices = pricesTask.Result;
            var securityList = portfolioData.Select(p => p.SecurityId).Distinct().ToList();
            if (securityList.Count > 0)
            {
                prices = prices.Where(p => securityList.Contains(p.SecurityId)).ToList();
            }

            Logger.LogInformation(default(EventId), message: "Prices filtered.");

            // calculate things.
            var data = this.Calculate(priceDate, portfolioData, prices);

            Logger.LogInformation(default(EventId), message: "Price Movement data Calculated.");

            return data;
        }

        /// <inheritdoc cref="IPriceMovements.GetLookupData"/>
        public async Task<LookupData> GetLookupData()
        {
            // get the lookup data
            var task = this.GetStaticData();
            await task.ConfigureAwait(false);

            var data = task.Result;

            return data;
        }

        /// <inheritdoc cref="IPriceMovements.Reset"/>
        public void Reset()
        {
            if (cts != null && !cts.IsCancellationRequested && cts.Token.CanBeCanceled)
            {
                cts.Cancel();
                cts.Dispose();
            }

            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Add the specified number of week days to the date specified.
        /// </summary>
        /// <param name="start">
        /// The start date.
        /// </param>
        /// <param name="days">
        /// The number of weekdays to add.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
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

        private static decimal GetBusinessDays(DateTime startD, DateTime endD)
        {
            var calcBusinessDays = (((endD - startD).TotalDays * 5) - ((startD.DayOfWeek - endD.DayOfWeek) * 2)) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday)
            {
                calcBusinessDays--;
            }

            if (startD.DayOfWeek == DayOfWeek.Sunday)
            {
                calcBusinessDays--;
            }

            return (decimal)calcBusinessDays;
        }

        private Task<List<PortfolioRecord>> GetPortolioData(DateTime runDate)
        {
            var cacheKey = string.Concat("PortfolioData", runDate.ToString("yyyyMMMdd", CultureInfo.InvariantCulture));

            // Look for cache key.
            if (this.cache.TryGetValue(cacheKey, out IEnumerable<PortfolioRecord> portfolioData))
            {
                return Task.Run(() =>
                    {
                        Logger.LogInformation(default(EventId), message: $"Data retrieved from cache: {portfolioData.Count()} records.");
                        return portfolioData.ToList();
                    });
            }

            return Task.Run(() =>
                {
                    // Key not in cache, so get data.
                    portfolioData = this.portfolioDataCommand.GetPortfolioData(runDate);

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.Normal).SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
                    cacheEntryOptions.AddExpirationToken(new CancellationChangeToken(cts.Token));

                    // Save data in cache.
                    this.cache.Set(cacheKey, portfolioData, cacheEntryOptions);

                    return portfolioData.ToList();
                });
        }

        private Task<List<PriceRecord>> GetPriceData(DateTime priceDate)
        {
            var cacheKey = string.Concat("PriceData", priceDate.ToString("yyyyMMMdd", CultureInfo.InvariantCulture));

            // Look for cache key.
            if (this.cache.TryGetValue(cacheKey, out IEnumerable<PriceRecord> priceData))
            {
                return Task.Run(() =>
                    {
                        Logger.LogInformation(default(EventId), message: $"Data retrieved from cache: {priceData.Count()} records.");
                        return priceData.ToList();
                    });
            }

            return Task.Run(() =>
                {
                    // Key not in cache, so get data.
                    priceData = this.priceDataCommand.GetPriceData(priceDate);

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.Normal).SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
                    cacheEntryOptions.AddExpirationToken(new CancellationChangeToken(cts.Token));

                    // Save data in cache.
                    this.cache.Set(cacheKey, priceData, cacheEntryOptions);

                    return priceData.ToList();
                });
        }

        private Task<LookupData> GetStaticData()
        {
            var cacheKey = "DropDown";

            // Look for cache key.
            if (this.cache.TryGetValue(cacheKey, out LookupData dropdownData))
            {
                return Task.Run(() =>
                    {
                        Logger.LogInformation(default(EventId), message: $"Data retrieved from cache: {dropdownData.AssetClasses} asset classes; {dropdownData.Currencies} currencies; {dropdownData.DealingDesks} dealing desks.");
                        return dropdownData;
                    });
            }

            return Task.Run(() =>
                {
                    // Key not in cache, so get data.
                    var lookups = this.staticDataCommand.GetStaticData();

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.Normal).SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
                    cacheEntryOptions.AddExpirationToken(new CancellationChangeToken(cts.Token));

                    // Save data in cache.
                    this.cache.Set(cacheKey, lookups, cacheEntryOptions);

                    return lookups;
                });
        }

        private List<PriceMovementRecord> Calculate(
            DateTime priceDate,
            List<PortfolioRecord> portfolioData,
            List<PriceRecord> currentPrices)
        {
            var priceMovementData = new List<PriceMovementRecord>();

            foreach (var portfolioRecord in portfolioData)
            {
                var prices = currentPrices.Where(s => s.SecurityId == portfolioRecord.SecurityId).ToList();

                var record = new PriceMovementRecord
                {
                    Portfolio = portfolioRecord.Portfolio,
                    PortfolioId = portfolioRecord.PortfolioId,
                    Description = portfolioRecord.Description,
                    SecurityId = portfolioRecord.SecurityId,
                    AssetClass = portfolioRecord.AssetClass,
                    AssetSubClass = portfolioRecord.AssetSubClass,
                    AssetType = portfolioRecord.AssetType,
                    AssetSubtype = portfolioRecord.AssetSubtype,
                    LegType = portfolioRecord.LegType,
                    Nominal = portfolioRecord.Nominal,
                    TotalValueUsd = portfolioRecord.TotalValueUsd,
                    TotalValueUsd1dActual = portfolioRecord.TotalValueUsd1dActual,
                    TotalValueUsdChange = portfolioRecord.TotalValueUsdChange,
                    PortfolioNav = portfolioRecord.PortfolioNav,
                    PortfolioNav1d = portfolioRecord.PortfolioNav1d,
                    PortfolioNavCustom = portfolioRecord.PortfolioNavCustom,
                    PortfolioNav1dCustom = portfolioRecord.PortfolioNav1dCustom,
                    ShareOfNav = portfolioRecord.ShareOfNav,
                    DealingDesk = portfolioRecord.DealingDesk,
                    RecordType = GetRecordType(portfolioRecord.RecordType),
                };

                var currentPrice = prices.SingleOrDefault(r => r.RowNum == 1);
                if (currentPrice != null)
                {
                    record.PriceCurrency = string.IsNullOrEmpty(currentPrice.PriceCurrency) ? portfolioRecord.PriceCurrency : currentPrice.PriceCurrency;
                    record.Price = (decimal?)(currentPrice.Price * currentPrice.ExternalFactor);
                    record.PriceDate = currentPrice.PricePoint;
                    record.PriceSource = currentPrice.Source;
                    record.BusinessDaysSincePriceDate = GetBusinessDays(currentPrice.PricePoint.Date, priceDate);
                }

                var previousPrice = prices.SingleOrDefault(r => r.RowNum == 2);
                if (previousPrice != null)
                {
                    record.Price1d = (decimal?)(previousPrice.Price * currentPrice.ExternalFactor);
                    record.PriceDate1d = previousPrice.PricePoint;
                    record.PriceSource1d = previousPrice.Source;
                }

                // no prices available (perhaps swaps)
                if (currentPrice != null && previousPrice != null)
                {
                    record.PriceChange = previousPrice.Price != 0.0 ? (decimal)((currentPrice.Price - previousPrice.Price) / previousPrice.Price) : (decimal)currentPrice.Price;
                    record.Proportion = record.PriceChange + 1;
                    record.TotalValueUsd1d = record.TotalValueUsd / record.Proportion;
                    record.ShareOfNav1d = record.TotalValueUsd1d / record.PortfolioNav1d;
                    record.ShareOfNavChange = record.ShareOfNav - record.ShareOfNav1d;
                    record.MarketValueImpact = (record.TotalValueUsd - record.TotalValueUsd1d) / record.PortfolioNav1d;
                }
                else
                {
                    // work out using data actually known
                    record.ShareOfNav1d = record.TotalValueUsd1dActual / record.PortfolioNav1d;
                    record.ShareOfNavChange = record.ShareOfNav - record.ShareOfNav1d;
                    record.MarketValueImpact = (record.TotalValueUsd - record.TotalValueUsd1dActual) / record.PortfolioNav1d;
                }

                priceMovementData.Add(record);
            }

            return priceMovementData;

            string GetRecordType(int recordType)
            {
                switch (recordType)
                {
                    case 1:
                        return "BOND";
                    case 2:
                        return "ETD";
                    case 3:
                        return "OTC";
                    default:
                        return "TODO";
                }
            }
        }
    }
}
