namespace PriceMovement.Infrastructure.Logging
{
    using System.DirectoryServices.AccountManagement;
    using System.IO;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using PriceMovement.Domain;
    using Serilog;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Exceptions;

    /// <summary>
    /// Configure Serilog.
    /// </summary>
    public class ConfigureLogging : IConfigureLogging
    {
        private readonly IHostingEnvironment environment;
        private readonly SerilogOptions options;
        private readonly ILogEventEnricher logEnricher;
        private LoggingLevelSwitch levelSwitch;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureLogging" /> class.
        /// </summary>
        /// <param name="environment">The hosting environment.</param>
        /// <param name="options">The Serilog config from app.settings.</param>
        /// <param name="logEnricher">The log enricher.</param>
        public ConfigureLogging(IHostingEnvironment environment, IOptions<SerilogOptions> options, ILogEventEnricher logEnricher)
        {
            this.environment = environment;
            this.options = options.Value;
            this.logEnricher = logEnricher;
        }

        /// <summary>
        /// Gets or sets the log level dynamically.
        /// </summary>
        public LogEventLevel LoggingEventLevel
        {
            get => this.levelSwitch.MinimumLevel;
            set => this.levelSwitch.MinimumLevel = value;
        }

        /// <summary>
        /// Configure Serilog for event sink with File, EventLog and Email sinks.
        /// </summary>
        public void Configure()
        {
            // set the initial logging level switch
            this.levelSwitch = new LoggingLevelSwitch { MinimumLevel = LogEventLevel.Information };

            // if development then get the developers email address .. stop spamming everyone
            string toEmail = this.options.EmailConnectionInfo.ToEmail;
            string fromEmail = this.options.EmailConnectionInfo.FromEmail;
            if (this.environment.IsDevelopment())
            {
                toEmail = UserPrincipal.Current.EmailAddress;
                fromEmail = toEmail;
            }

            // build email options
            var emailOptions =
                new Serilog.Sinks.Email.EmailConnectionInfo
                {
                    MailServer = this.options.EmailConnectionInfo.MailServer,
                    Port = this.options.EmailConnectionInfo.Port,
                    EnableSsl = false,
                    FromEmail = fromEmail,
                    ToEmail = toEmail,
                    EmailSubject = $"Error : [{this.environment.EnvironmentName}] - PriceMovement ",
                };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(this.levelSwitch)
                .Enrich.FromLogContext()
                .Enrich.With(this.logEnricher)
                .Enrich.WithThreadId()
                .Enrich.WithExceptionDetails()
                .WriteTo.RollingFile(Path.Combine(this.options.LogFileLocation, this.options.LogFilename), outputTemplate: this.options.OutputTemplate)
                .WriteTo.EventLog(source: "PriceMovement", restrictedToMinimumLevel: LogEventLevel.Error)
                .WriteTo.Email(connectionInfo: emailOptions, restrictedToMinimumLevel: LogEventLevel.Error, batchPostingLimit: this.options.EmailConnectionInfo.BatchPostingLimit ?? 10)
                .CreateLogger();
        }
    }
}
