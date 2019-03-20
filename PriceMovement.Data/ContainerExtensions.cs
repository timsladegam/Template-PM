namespace PriceMovement.Data
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
        public static IServiceCollection RegisterDataServices(this IServiceCollection services)
        {
            // Context Providers
            services.AddScoped<IPortfolioDataCommand, PortfolioDataCommand>();
            services.AddScoped<IPriceDataCommand, PriceDataCommand>();
            services.AddScoped<IStaleYieldDataCommand, StaleYieldDataCommand>();
            services.AddScoped<IUnderlyingDataCommand, UnderlyingDataCommand>();
            services.AddScoped<IYieldPointDataCommand, YieldPointDataCommand>();
            services.AddScoped<IStaticDataCommand, StaticDataCommand>();
            return services;
        }
    }
}
