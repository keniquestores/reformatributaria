using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace ReformaTributaria.Infra.IoC
{
    internal static class ContainerApiVersioning
    {
        internal static IServiceCollection ApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddMvc()
              .AddApiExplorer(
                    options =>
                    {
                        options.GroupNameFormat = "'v'VVV";
                        options.SubstituteApiVersionInUrl = true;
                    });

            return services;
        }
    }
}
