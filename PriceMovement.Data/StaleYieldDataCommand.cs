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
    /// The stale yield data command.
    /// </summary>
    public class StaleYieldDataCommand : IStaleYieldDataCommand
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaleYieldDataCommand"/> class.
        /// </summary>
        /// <param name="options">Connection strings.</param>
        public StaleYieldDataCommand(IOptions<ConnectionStrings> options)
        {
            this.connectionString = options.Value.GRDBContext;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static ILogger Logger => AppLogging.CreateLogger<StaleYieldDataCommand>();

        /// <summary>
        /// Get underlying data command.
        /// </summary>
        /// <param name="date">The run date.</param>
        /// <returns>An enumerable of stale yield records.</returns>
        public IEnumerable<StaleYieldRecord> GetStaleYieldData(DateTime date)
        {
            IEnumerable<StaleYieldRecord> staleYieldRecords;
            var sql = SQLScripts.GRDBGetStaleYieldsStoredProc;

            Logger.LogInformation(default(EventId), message: "Calling StaleYieldDataCommand.GetPortfolioData");
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                staleYieldRecords = connection.Query<StaleYieldRecord>(sql, new { priceDate = date }, commandType: CommandType.Text, commandTimeout: 300);
            }

            Logger.LogInformation(default(EventId), message: $"Exit StaleYieldDataCommand.GetStaleYieldData retrieved: {staleYieldRecords.Count()} records.");

            return staleYieldRecords;
        }
    }
}
