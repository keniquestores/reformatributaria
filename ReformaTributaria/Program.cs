using IdentityServer.API.Middlewares;
using Microsoft.AspNetCore.Authorization;
using ReformaTributaria.Filters;
using ReformaTributaria.Handlers;
using ReformaTributaria.Infra.IoC;

namespace ReformaTributaria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext(builder.Configuration)
                .AddApplicationServices(builder.Configuration)
                .AddInfrastructure()
                .AddControllers();

            builder.Services.AddAppSettings(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API Reforma Tributária",
                    Version = "v1",
                    Description = "API para gerenciamento de clientes - Reforma Tributária"
                });
            });

            builder.Services.AddScoped<ResultActionFilter>();

            builder.Services.AddAuthorizationBuilder().AddPolicy("IsProductPolicy", policy => policy.Requirements.Add(new IsProductRequirement()));

            builder.Services.AddScoped<IAuthorizationHandler, IsProductAuthorizationHandler>();

            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationResultHandler>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Reforma Tributária API v1");
                });
            }
            app.UseApiKey();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
