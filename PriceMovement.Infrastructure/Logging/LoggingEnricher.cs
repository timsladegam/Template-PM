namespace PriceMovement.Infrastructure.Logging
{
    using Microsoft.AspNetCore.Http;

    using Serilog.Core;
    using Serilog.Events;

    /// <summary>
    /// The logger enricher class.
    /// </summary>
    public class LoggingEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingEnricher"/> class.
        /// The logging enricher.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public LoggingEnricher(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Enrich log with a correlation id and user name from the http context.
        /// </summary>
        /// <param name="logEvent">The log event to be enriched.</param>
        /// <param name="propertyFactory">The property factory.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            // check the static httpcontext object exists
            var userName = this.httpContextAccessor?.HttpContext?.User.Identity.Name;
            var traceIdentifier = this.httpContextAccessor?.HttpContext?.TraceIdentifier;

            // add the correlation id
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", traceIdentifier));

            // add the user name
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserId", userName));
        }
    }
}
