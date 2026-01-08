using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReformaTributaria.Application.Services;
using ReformaTributaria.Application.Services.Interfaces;
using ReformaTributaria.Application.Validators;
using ReformaTributaria.Application.Validators.Dtos;
using ReformaTributaria.CrossCutting.ExecutionContext.Extensions;
using ReformaTributaria.Domain.Common;
using ReformaTributaria.Domain.Interfaces.Infra.Repositories;
using ReformaTributaria.Infra.Context;
using ReformaTributaria.Infra.Repositories;

namespace ReformaTributaria.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IFilaService, FilaService>();
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.AddScoped<IIdentityService, IdentityService>();


            services.AddRequestScope().AddRequestScopeEfCore<ReformaTributariaDbContext>();

            services.AddAutoMapper(
                (serviceProvider, config) =>
                {
                    config.AddMaps(Assembly.Load("ReformaTributaria.Application"));

                    var appSettings = serviceProvider.GetRequiredService<AppSettings>();

                    config.ConstructServicesUsing(type =>
                    {
                        return serviceProvider.GetService(type) ?? Activator.CreateInstance(type);
                    });

                },
                [Assembly.Load("ReformaTributaria.Application")]
            );

            ContainerApiVersioning.ApiVersioning(services);

            services.AddApplicationAuthentication(configuration);


            services.AddValidators();

            return services;
        }

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IFilaRepository, FilaRepository>();

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReformaTributariaDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null
                    );
                }));

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped(typeof(IValidator<ClienteDtoValidator>), typeof(ClienteValidator));

            return services;
        }

        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            services.AddSingleton(appSettings);

            return services;
        }
    }
}
