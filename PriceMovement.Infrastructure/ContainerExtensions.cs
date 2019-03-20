namespace PriceMovement.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    using PriceMovement.Domain;
    using PriceMovement.Infrastructure.HealthChecks;
    using PriceMovement.Infrastructure.Logging;

    using Serilog.Core;

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
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
        {
            // Context Providers
            services.AddSingleton<IHealthCheck, WebSiteHealthCheck>();
            services.AddScoped<ILogEventEnricher, LoggingEnricher>();

            // we need to add this long hand so resolve some bits first
            var provider = services.BuildServiceProvider();
            var logOptions = provider.GetRequiredService<IOptions<SerilogOptions>>();
            var hosting = provider.GetRequiredService<IHostingEnvironment>();
            var logEnricher = provider.GetRequiredService<ILogEventEnricher>();

            // create a new instance and register
            var configureLogging = new ConfigureLogging(hosting, logOptions, logEnricher);
            services.AddSingleton<IConfigureLogging>(configureLogging);

            // now configure
            configureLogging.Configure();

            return services;
        }
    }
}