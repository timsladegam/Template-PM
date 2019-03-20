namespace PriceMovement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PriceMovement.Business;
    using PriceMovement.Domain;
    using PriceMovement.Infrastructure.Logging;

    /// <summary>
    /// The price movement controller.
    /// </summary>
    [Route("api/[controller]")]
    public class PriceMovementController : Controller
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static ILogger Logger => AppLogging.CreateLogger<PriceMovementController>();

        /// <summary>
        /// Gets the price movements.
        /// </summary>
        /// <param name="priceMovement">The price movements service.</param>
        /// <param name="data">The client data.</param>
        /// <returns>The price movement data.</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Data([FromServices] IPriceMovements priceMovement, [FromBody] PostData data)
        {
            try
            {
                var runDate = data.ForDate ?? DateTime.Today.Date;

                Logger.LogInformation(default(EventId), message: $"Retrieving PriceMovement Data for {runDate}; report type: {data.RecordType}; assets classes: {data.AssetClasses}; currencies: {data.Currencies}; dealing desks: {data.DealingDesks}.");

                var result = await priceMovement
                                 .GetPriceMovementRecords(runDate, data.AssetClasses, data.Currencies, data.DealingDesks, data.RecordType)
                                 .ConfigureAwait(false);

                Logger.LogInformation(default(EventId), message: $"Exit Data: retrieved {result.Count} records.");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(default(EventId), message: "Unexpected Exception:", exception: ex);
                return this.StatusCode(500, $"Unexpected Exception: {ex}");
            }
        }

        /// <summary>
        /// Gets the drop down data.
        /// </summary>
        /// <param name="priceMovement">The price movements service.</param>
        /// <returns>Teh drop down data.</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> DropDownData([FromServices] IPriceMovements priceMovement)
        {
            try
            {
                Logger.LogInformation(default(EventId), message: $"Retrieving Dropdown Lists.");

                var result = await priceMovement
                                 .GetLookupData()
                                 .ConfigureAwait(false);

                Logger.LogInformation(default(EventId), message: $"Exit DropDownData: retrieved {result.AssetClasses.Count} asset classes;  {result.Currencies.Count} currencies; {result.DealingDesks.Count} dealing desks.");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(default(EventId), message: "Unexpected Exception:", exception: ex);
                return this.StatusCode(500, $"Unexpected Exception: {ex}");
            }
        }

        /// <summary>
        /// Gets the prices.
        /// </summary>
        /// <param name="prices">The prices service.</param>
        /// <param name="securityId">The security id.</param>
        /// <param name="forDate">The for date.</param>
        /// <param name="dayCount">The day count.</param>
        /// <returns>The price data.</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> Prices([FromServices] IPrices prices, string securityId, DateTime? forDate = null, int dayCount = 5)
        {
            try
            {
                var runDate = forDate ?? DateTime.Today.Date;

                Logger.LogInformation(default(EventId), message: $"Retrieving Prices for {runDate}; security: {securityId}; for days: {dayCount}.");

                var result = await prices
                                               .GetPriceRecords(securityId, runDate, dayCount)
                                               .ConfigureAwait(false);

                Logger.LogInformation(default(EventId), message: $"Exit Prices: retrieved {result.Count} records.");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(default(EventId), message: "Unexpected Exception:", exception: ex);
                return this.StatusCode(500, $"Unexpected Exception: {ex}");
            }
        }

        /// <summary>
        /// Gets the yield points.
        /// </summary>
        /// <param name="yieldPoint">The yield point service.</param>
        /// <param name="data">The post data.</param>
        /// <returns>The yield point data.</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> YieldPoint([FromServices] IYieldPoint yieldPoint, [FromBody] PostData data)
        {
            try
            {
                var runDate = data.ForDate ?? DateTime.Today.Date;

                Logger.LogInformation(default(EventId), message: $"Retrieving Yield Point data for {runDate}; assets classes: {data.AssetClasses}; currencies: {data.Currencies}.");

                var result = await yieldPoint
                                 .GetYieldPointRecords(runDate, data.AssetClasses, data.Currencies)
                                 .ConfigureAwait(false);

                Logger.LogInformation(default(EventId), message: $"Exit YieldPoint: retrieved {result.Count} records.");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(default(EventId), message: "Unexpected Exception:", exception: ex);
                return this.StatusCode(500, $"Unexpected Exception: {ex}");
            }
        }

        /// <summary>
        /// Gets the stale yields.
        /// </summary>
        /// <param name="staleYield">The stale yield service.</param>
        /// <param name="data">The post data.</param>
        /// <returns>The stale yield data.</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Stale([FromServices] IStaleYield staleYield, [FromBody] PostData data)
        {
            try
            {
                var runDate = data.ForDate ?? DateTime.Today.Date;

                Logger.LogInformation(default(EventId), message: $"Retrieving Stale Yields data for {runDate}; assets classes: {data.AssetClasses}; currencies: {data.Currencies}.");

                var result = await staleYield
                                 .GetStaleYieldRecords(runDate, data.AssetClasses, data.Currencies)
                                 .ConfigureAwait(false);

                Logger.LogInformation(default(EventId), message: $"Exit Stale: retrieved {result.Count} records.");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(default(EventId), message: "Unexpected Exception:", exception: ex);
                return this.StatusCode(500, $"Unexpected Exception: {ex}");
            }
        }

        /// <summary>
        /// Gets the underlying.
        /// </summary>
        /// <param name="underlying">The underlying service.</param>
        /// <param name="data">The post data.</param>
        /// <returns>The underlying data.</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Underlying([FromServices] IUnderlying underlying, [FromBody] PostData data)
        {
            try
            {
                var runDate = data.ForDate ?? DateTime.Today.Date;

                Logger.LogInformation(default(EventId), message: $"Retrieving Underlying Instrument data for {runDate}; currencies: {data.Currencies}.");

                var result = await underlying
                                 .GetUnderlyingRecords(runDate, data.Currencies)
                                 .ConfigureAwait(false);

                Logger.LogInformation(default(EventId), message: $"Exit Underlying: retrieved {result.Count} records.");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(default(EventId), message: "Unexpected Exception:", exception: ex);
                return this.StatusCode(500, $"Unexpected Exception: {ex}");
            }
        }

        /// <summary>
        /// Reset any cached data.
        /// </summary>
        /// <param name="priceMovement">The price movement service.</param>
        /// <returns>Success.</returns>
        [HttpGet("[action]")]
        public IActionResult ResetCache([FromServices] IPriceMovements priceMovement)
        {
            try
            {
                Logger.LogInformation(default(EventId), message: "Resetting cache.");

                var cacheReset = new Dictionary<string, object> { { "success", true } };
                var reply = new Dictionary<string, object> { { "data", cacheReset } };

                // reset known caches
                priceMovement.Reset();

                Logger.LogInformation(default(EventId), message: "Exit Reset cache.");
                return new OkObjectResult(reply);
            }
            catch (Exception ex)
            {
                Logger.LogError(default(EventId), message: "Unexpected Exception:", exception: ex);
                return this.StatusCode(500, $"Unexpected Exception: {ex}");
            }
        }

        /// <summary>
        /// Log a client error.
        /// </summary>
        /// <param name="logEntry">The error to log.</param>
        /// <returns>Success.</returns>
        [HttpPost("[action]")]
        public IActionResult LogClient(dynamic logEntry)
        {
            Logger.LogError($"Console Client App Error, User: {this.User.Identity.Name} \n {logEntry}");
            return new OkResult();
        }
    }
}
