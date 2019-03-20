namespace PriceMovement.Infrastructure.Middleware
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Primitives;

    using PriceMovement.Domain;

    /// <summary>
    /// The correlation middleware.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate next;
        private readonly CorrelationIdOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="options">The configuration options.</param>
        public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.next = next ?? throw new ArgumentNullException(nameof(next));

            this.options = options.Value;
        }

        /// <summary>
        /// Processes a request to synchronise TraceIdentifier and Correlation ID headers .
        /// </summary>
        /// <param name="context">The HttpContext.</param>
        /// <returns>The invoked task.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(this.options.Header, out StringValues correlationId))
            {
                context.TraceIdentifier = correlationId;
            }

            if (this.options.IncludeInResponse)
            {
                // apply the correlation ID to the response header for client side tracking
                context.Response.OnStarting(
                    () =>
                    {
                        if (context.Response.Headers.ContainsKey(this.options.Header))
                        {
                            context.Response.Headers.Add(this.options.Header, new[] { context.TraceIdentifier });
                        }

                        return Task.CompletedTask;
                    });
            }

            await this.next(context).ConfigureAwait(false);
        }
    }
}
