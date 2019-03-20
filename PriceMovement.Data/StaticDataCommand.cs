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
    /// <inheritdoc cref="IStaticDataCommand"/>
    /// </summary>
    public class StaticDataCommand : IStaticDataCommand
    {
        private readonly string connectionString;
        private readonly int timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticDataCommand"/> class.
        /// </summary>
        /// <param name="options">Connection strings.</param>
        public StaticDataCommand(IOptions<ConnectionStrings> options)
        {
            this.connectionString = options.Value.GRDBContext;
            this.timeout = options.Value.DataBaseCommandTimeout;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static ILogger Logger => AppLogging.CreateLogger<StaticDataCommand>();

        /// <inheritdoc cref="IStaticDataCommand.GetStaticData"/>
        public LookupData GetStaticData()
        {
            var lookupData = new LookupData();
            var sql = SQLScripts.GRDBGetStaticStoredProc;

            Logger.LogInformation(default(EventId), message: "Calling StaticDataCommand.GetStaticData");

            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                var reader = connection.QueryMultiple(sql, commandType: CommandType.Text, commandTimeout: this.timeout);

                lookupData.AssetClasses.AddRange(reader.Read<string>().ToList());
                lookupData.Currencies.AddRange(reader.Read<string>().ToList());
                lookupData.DealingDesks.AddRange(reader.Read<string>().ToList());
            }

            Logger.LogInformation(default(EventId), message: $"Exit StaticDataCommand.GetStaticData.");

            return lookupData;
        }
    }
}
