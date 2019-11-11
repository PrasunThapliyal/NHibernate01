
namespace ORMWebAPI
{
    using System;
    using Ciena.BluePlanet.OnePlannerRestLibrary;
    using Ciena.BluePlanet.TopologyPlanningService.Utilities;
    using CommonServiceLocator;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Practices.Prism.Events;
    using OnePlanner.CommonCS.Commands;
    using OnePlanner.CommonCS.DummyWrappers.DbWrapper;
    using OnePlanner.CommonCS.Logging;
    using OnePlanner.DBInterface.DBInterface;

    public class Startup
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;

        public Startup(
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = _loggerFactory.CreateLogger<Startup>();
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // OnePlanner Stuff
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILog>(new OnePlannerLoggingWrapper(_logger));
            services.AddSingleton<IUndoManagerFactory, UndoManagerFactory>();
            services.AddSingleton<IEventAggregatorFactory, EventAggregatorFactory>();
            services.AddScoped<IEventAggregator, EventAggregator>();
            services.AddScoped<IUndoManager, UndoManager>();

            PopulateContainer(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            SetServiceLocator(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void PopulateContainer(IServiceCollection services)
        {
            services.AddTransient<IExtendedCDBServerDetails, ExtendedCDBServerDetails>();
        }

        private void SetServiceLocator(IApplicationBuilder app)
        {
            var serviceProviderKey = "ServiceProvider";
            ServiceLocator.SetLocatorProvider(() =>
            {
                var httpContextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>();
                if (httpContextAccessor.HttpContext == null)
                {
                    return new ServiceProviderServiceLocator(app.ApplicationServices.GetRequiredService<IServiceProvider>());
                }
                if (httpContextAccessor.HttpContext.Items[serviceProviderKey] == null)
                {
                    var serviceProvider = httpContextAccessor.HttpContext.RequestServices;
                    httpContextAccessor.HttpContext.Items[serviceProviderKey] = new ServiceProviderServiceLocator(serviceProvider);
                }
                return httpContextAccessor.HttpContext.Items[serviceProviderKey] as IServiceLocator;
            });
        }

    }
}
