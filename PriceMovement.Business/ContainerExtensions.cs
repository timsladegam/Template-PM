namespace PriceMovement.Business
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The container extensions.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Register services in the DI container.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The updated services collection.</returns>
        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
        {
            // Context Providers
            services.AddScoped<IPriceMovements, PriceMovements>();
            services.AddScoped<IPrices, Prices>();
            services.AddScoped<IStaleYield, StaleYield>();
            services.AddScoped<IUnderlying, Underlying>();
            services.AddScoped<IYieldPoint, YieldPoint>();

            return services;
        }
    }
}