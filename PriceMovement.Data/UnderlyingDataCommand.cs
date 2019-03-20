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
    /// The underlying data command.
    /// </summary>
    public class UnderlyingDataCommand : IUnderlyingDataCommand
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnderlyingDataCommand"/> class.
        /// </summary>
        /// <param name="options">Connection strings.</param>
        public UnderlyingDataCommand(IOptions<ConnectionStrings> options)
        {
            this.connectionString = options.Value.GRDBContext;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static ILogger Logger => AppLogging.CreateLogger<UnderlyingDataCommand>();

        /// <summary>
        /// Get underlying data command.
        /// </summary>
        /// <param name="date">The run date.</param>
        /// <returns>An enumerable of underlying records.</returns>
        public IEnumerable<UnderlyingRecord> GetUnderlyingData(DateTime date)
        {
            IEnumerable<UnderlyingRecord> underlyingRecords;
            var sql = SQLScripts.GRDBGetUnderlyingStoredProc;

            Logger.LogInformation(default(EventId), message: "Calling UnderlyingDataCommand.GetPortfolioData");
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                underlyingRecords = connection.Query<UnderlyingRecord>(sql, new { priceDate = date }, commandType: CommandType.Text, commandTimeout: 300);
            }

            Logger.LogInformation(default(EventId), message: $"Exit UnderlyingDataCommand.GetUnderlyingData retrieved: {underlyingRecords.Count()} records.");

            return underlyingRecords;
        }
    }
}
