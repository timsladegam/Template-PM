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
    /// <inheritdoc cref="IPortfolioDataCommand"/>
    /// </summary>
    public class PortfolioDataCommand : IPortfolioDataCommand
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioDataCommand"/> class.
        /// </summary>
        /// <param name="options">Connection strings.</param>
        public PortfolioDataCommand(IOptions<ConnectionStrings> options)
        {
            this.connectionString = options.Value.GRDBContext;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static ILogger Logger => AppLogging.CreateLogger<PortfolioDataCommand>();

        /// <inheritdoc cref="IPortfolioDataCommand.GetPortfolioData"/>
        public IEnumerable<PortfolioRecord> GetPortfolioData(DateTime date)
        {
            IEnumerable<PortfolioRecord> portfolioData;
            var sql = SQLScripts.GRDBGetPriceMovementStoredProc;

            Logger.LogInformation(default(EventId), message: "Calling PortfolioDataCommand.GetPortfolioData");

            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                portfolioData = connection.Query<PortfolioRecord>(sql, new { priceDate = date }, commandType: CommandType.Text, commandTimeout: 300);
            }

            Logger.LogInformation(default(EventId), message: $"Exit PortfolioDataCommand.GetPortfolioData retrieved: {portfolioData.Count()} records.");

            return portfolioData;
        }
    }
}
