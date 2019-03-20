namespace PriceMovement.Infrastructure.Logging
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The application logging static.
    /// </summary>
    public static class AppLogging
    {
        /// <summary>
        /// Gets the new logger factory.
        /// </summary>
        public static ILoggerFactory LoggerFactory { get; } = new LoggerFactory();

        /// <summary>
        /// Create a new Ilogger of type T.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The ILogger.</returns>
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    }
}