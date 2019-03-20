namespace PriceMovement.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Dapper;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using PriceMovement.Data.SQL;
    using PriceMovement.Domain;
    using PriceMovement.Infrastructure.Logging;

    /// <summary>
    /// The price data command.
    /// </summary>
    public class PriceDataCommand : IPriceDataCommand
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceDataCommand"/> class.
        /// </summary>
        /// <param name="options">Connection strings.</param>
        public PriceDataCommand(IOptions<ConnectionStrings> options)
        {
            this.connectionString = options.Value.ThinkfolioContext;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static ILogger Logger => AppLogging.CreateLogger<PriceDataCommand>();

        /// <summary>
        /// Get prices for date for list of security ids.
        /// </summary>
        /// <param name="date">The run date.</param>
        /// <returns>The price data.</returns>
        public IEnumerable<PriceRecord> GetPriceData(DateTime date)
        {
            IEnumerable<PriceRecord> priceData;
            var sql = SQLScripts.ThinkFolioSelectPrices;

            Logger.LogInformation(default(EventId), message: "Calling PriceDataCommand.GetPriceData");
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                priceData = connection.Query<PriceRecord>(sql, new { PriceDate = date }, commandType: CommandType.Text, commandTimeout: 300).ToList();
            }

            Logger.LogInformation(default(EventId), message: $"Exit PriceDataCommand.GetPriceData retrieved: {priceData.Count()} records.");

            return priceData;
        }

        /// <summary>
        /// Get all prices for a security id between a from and to date.
        /// </summary>
        /// <param name="securityId">The security id.</param>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <returns>The price data.</returns>
        public IEnumerable<PriceRecord> GetPriceData(string securityId, DateTime fromDate, DateTime toDate)
        {
            IEnumerable<PriceRecord> priceData;
            var sql = SQLScripts.ThinkFolioSelectPriceHistory;

            Logger.LogInformation(default(EventId), message: "Calling PriceDataCommand.GetPriceData");
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                priceData = connection.Query<PriceRecord>(sql, new { SecurityId = securityId, FromDate = fromDate, ToDate = toDate }, commandType: CommandType.Text, commandTimeout: 300);
            }

            Logger.LogInformation(default(EventId), message: $"Exit PriceDataCommand.GetPriceData retrieved: {priceData.Count()} records.");

            return priceData;
        }
    }
}
