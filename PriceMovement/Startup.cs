namespace PriceMovement
{
    using System.IO;
    using Dapper.FluentMap;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Logging;
    using PriceMovement.Business;
    using PriceMovement.Data;
    using PriceMovement.Domain;
    using PriceMovement.Infrastructure;
    using PriceMovement.Infrastructure.HealthChecks;
    using PriceMovement.Infrastructure.Logging;
    using PriceMovement.Infrastructure.Middleware;
    using Serilog;
    using Swashbuckle.AspNetCore.Swagger;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Teh configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        private ILogger Logger { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Configure Option using Extensions method
            services.Configure<ConnectionStrings>(this.Configuration.GetSection("ConnectionStrings"));
            services.Configure<SerilogOptions>(this.Configuration.GetSection("Serilog"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // add system service.
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            // health checks
            services.AddHealthChecksUI();
            services.AddHealthChecks()
                .AddCheck<WebSiteHealthCheck>("random")
                .AddSqlServer(connectionString: this.Configuration.GetConnectionString("GRDBContext"), name: "grdb", failureStatus: HealthStatus.Unhealthy, tags: new[] { "db", "sql", "sqlserver", "grdb" })
                .AddSqlServer(connectionString: this.Configuration.GetConnectionString("ThinkfolioContext"), name: "tf", failureStatus: HealthStatus.Unhealthy, tags: new[] { "db", "sql", "sqlserver", "thinkfolio" });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                });

            // register user services
            services.RegisterInfrastructureServices();
            services.RegisterDataServices();
            services.RegisterBusinessServices();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Add service and create Policy with options
            services.AddCors(options =>
                {
                    options.AddPolicy(
                        "CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The environment.</param>
        /// <param name="applicationLifetime">The application lifetime.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            // set the current directory to the environment content root ... note this is only for AspNet.Core 2.2 ... not need for AspNet.Core 3.0 as issue is fixed.
            Directory.SetCurrentDirectory(env.ContentRootPath);

            // set up  logging
            AppLogging.LoggerFactory.AddSerilog();
            this.Logger = AppLogging.CreateLogger<Startup>();

            // register to flush logs if shutdown called
            applicationLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            // log the environment on startup
            this.Logger.LogInformation($"Environment is {env.EnvironmentName}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // initialise health checks
            app.UseHealthChecks(
                "/health",
                new HealthCheckOptions() { Predicate = _ => true });
            app.UseHealthChecks(
                "/healthchecks",
                options: new HealthCheckOptions() { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
            app.UseHealthChecksUI(
                setup =>
                    {
                        // defaults
                        setup.ApiPath = "/healthchecks-api";
                        setup.UIPath = "/healthchecks-ui";
                        setup.WebhookPath = "/healthchecks-webhooks";
                        setup.ResourcesPath = "/ui/resources";
                    });

            // add middleware
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSwagger();
            app.UseCorrelationId();

            FluentMapper.Initialize(config =>
                {
                    config.AddMap(new PriceMap());
                    config.AddMap(new PortfolioMap());
                    config.AddMap(new StaleYieldMap());
                    config.AddMap(new UnderlyingMap());
                    config.AddMap(new YieldPointMap());
                });

            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PriceMovement");
                });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                //// To learn more about options for serving an Angular SPA from ASP.NET Core,
                //// see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
