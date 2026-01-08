using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ReformaTributaria.Application.Services.Interfaces;

namespace ReformaTributaria.Infra.IoC
{
    public static class ContainerHangfire
    {
        public static IServiceCollection AddPostgreSQLHangFire(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(configuration =>
            {
                configuration.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString));
            });

            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.ServerName = "Hangfire Reforma Tributária";
                serverOptions.Queues = ["default"];
            });

            GlobalConfiguration.Configuration.UseSerializerSettings(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return services;
        }

        public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Dashboard Reforma Tributária",
                Authorization = [ new HangfireCustomBasicAuthenticationFilter {
                    User = configuration.GetValue<string>("Hangfire:Usuario"),
                    Pass = configuration.GetValue<string>("Hangfire:Senha"),
                } ]
            });

            return app;
        }

        public static IApplicationBuilder UseHangfireRecurringJobs(this IApplicationBuilder app, IRecurringJobManager jobManager)
        {
            jobManager.AddOrUpdate<IReformaTributariaConsumoService>("Executar Apurações Cbs", x => x.ExecutarApuracoesCbsAsync(), Cron.Daily(2));
            return app;
        }
    }
}
