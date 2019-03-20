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
    /// The yield point data command.
    /// </summary>
    public class YieldPointDataCommand : IYieldPointDataCommand
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="YieldPointDataCommand"/> class.
        /// </summary>
        /// <param name="options">Connection strings.</param>
        public YieldPointDataCommand(IOptions<ConnectionStrings> options)
        {
            this.connectionString = options.Value.GRDBContext;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static ILogger Logger => AppLogging.CreateLogger<YieldPointDataCommand>();

        /// <summary>
        /// Get yield point data command.
        /// </summary>
        /// <param name="date">The run date.</param>
        /// <returns>An enumerable of yield point records.</returns>
        public IEnumerable<YieldPointRecord> GetYieldPointData(DateTime date)
        {
            IEnumerable<YieldPointRecord> yieldPointRecords;
            var sql = SQLScripts.GRDBGetYieldPointsStoredProc;

            Logger.LogInformation(default(EventId), message: "Calling YieldPointDataCommand.GetPortfolioData");
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                yieldPointRecords = connection.Query<YieldPointRecord>(sql, new { priceDate = date }, commandType: CommandType.Text, commandTimeout: 300);
            }

            Logger.LogInformation(default(EventId), message: $"Exit YieldPointDataCommand.GetYieldPointData retrieved: {yieldPointRecords.Count()} records.");

            return yieldPointRecords;
        }
    }
}
