using Domain.Finances.BusinessAggregate;
using Domain.Finances.CategoryAggregate;
using Domain.Finances.MoneyActivityAggregate;
using Domain.Finances.StandingOrderAggregate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.Services;
using Common.JsonDatabase.Cache;
using Common.JsonDatabase.DatabaseObject;
using System;
using System.Threading.Tasks;
using Common.TaskScheduler;

namespace MyFinances
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var serviceConfiguration = InitServiceConfiguration();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers();
            services.WithJsonDatabaseOptions(DatabaseObjectServiceOptions.Create(serviceConfiguration.BaseDirectory, serviceConfiguration.StorageFolder));
            services.AddSingleton<IDatabaseObjectServiceFactory, DatabaseObjectServiceFactory>();
            services.AddSingleton<IStandingOrderService, StandingOrderService>();
            services.AddSingleton<ITaskScheduler, Common.TaskScheduler.TaskScheduler>();
            services.AddSingleton<TaskActivator, TaskActivator>();

            InitializeCacheAsync().ConfigureAwait(true);

            InitializeScheduler(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private MyFinancesServiceConfiguration InitServiceConfiguration()
        {
            var serviceConfigurationFileName = Configuration.GetValue<string>("ServiceConfiguration");
            if (string.IsNullOrWhiteSpace(serviceConfigurationFileName))
            {
                throw new Exception("ServiceConfiguration key is not given in the appsettings.");
            }

            return ServiceConfigurationReader.GetServiceConfiguration<MyFinancesServiceConfiguration>(serviceConfigurationFileName);
        }

        private static async Task InitializeCacheAsync()
        {
            // Configure cache
            DatabaseObjectCache<Business>.ConfigureEvergrowingCache();
            DatabaseObjectCache<Category>.ConfigureEvergrowingCache();
            DatabaseObjectCache<StandingOrder>.ConfigureEvergrowingCache();

            DatabaseObjectCache<MoneyActivity>.ConfigureFixedCache(1024 * 10);

            // Init cache
            await DatabaseObjectCache<Business>.InitializeEvergrowingCacheAsync().ConfigureAwait(false);
            await DatabaseObjectCache<Category>.InitializeEvergrowingCacheAsync().ConfigureAwait(false);
            await DatabaseObjectCache<StandingOrder>.InitializeEvergrowingCacheAsync().ConfigureAwait(false);

            await DatabaseObjectCache<MoneyActivity>.InitializeFixedCacheAsync().ConfigureAwait(false);
        }

        private static void InitializeScheduler(IServiceCollection services)
        {
            var standingOrderService = services.BuildServiceProvider().GetRequiredService<IStandingOrderService>();
            var taskRunner = services.BuildServiceProvider().GetRequiredService<ITaskScheduler>();

            var standingOrderTask = PeriodicTask.Create(
                async () =>
                {
                    await standingOrderService.ExcecuteStandingOrders().ConfigureAwait(false);
                },
                "Standing order execution service",
                true,
                "00:01:00");

            taskRunner.AddTask(standingOrderTask);
            taskRunner.Start(TimeSpan.FromSeconds(30));
        }
    }
}
