using Hangfire;
using ReformaTributaria.Application.Services;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Interfaces;
using ReformaTributaria.Domain.Common;
using ReformaTributaria.Infra.IoC;

namespace ReformaTributaria.HangFire
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPostgreSQLHangFire(Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")!);

            //services.AddApplicationHangfireCollections(Configuration);

            services.Configure<AmbienteReceitaOptions>(Configuration.GetSection("AmbienteReceita"));

            services.AddHttpClient<IReformaTributariaConsumoService, ReformaTributariaConsumoService>();

            services.AddAppSettings(Configuration);

            services.AddDbContext(Configuration);

            services.AddApplicationServices(Configuration);

            services.AddInfrastructure();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager jobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseHangfireDashboard(Configuration);

            app.UseHangfireRecurringJobs(jobManager);
        }


    }
}
