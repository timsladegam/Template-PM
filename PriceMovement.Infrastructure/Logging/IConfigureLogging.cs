namespace PriceMovement.Infrastructure.Logging
{
    using Serilog.Events;

    /// <summary>
    /// Configure Logging interface.
    /// </summary>
    public interface IConfigureLogging
    {
        /// <summary>
        /// Gets or sets the log event level.
        /// </summary>
        LogEventLevel LoggingEventLevel { get; set; }

        /// <summary>
        /// Configure the logging.
        /// </summary>
        void Configure();
    }
}
