namespace PriceMovement.Infrastructure.Middleware
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Options;
    using PriceMovement.Domain;

    /// <summary>
    /// Extension methods for the CorrelationIdMiddleware.
    /// </summary>
    public static class CorrelationIdExtensions
    {
        /// <summary>
        /// Enables correlation IDs for the request.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>Returns the application builder.</returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<CorrelationIdMiddleware>();
        }

        /// <summary>
        /// Enables correlation IDs for the request.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="header">The header field name to use for the correlation ID.</param>
        /// <returns>Returns the application builder.</returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, string header)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseCorrelationId(new CorrelationIdOptions { Header = header });
        }

        /// <summary>
        /// Enables correlation IDs for the request.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="options">The correlation options.</param>
        /// <returns>Returns the application builder.</returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, CorrelationIdOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<CorrelationIdMiddleware>(Options.Create(options));
        }
    }
}
