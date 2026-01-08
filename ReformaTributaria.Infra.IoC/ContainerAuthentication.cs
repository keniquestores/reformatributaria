using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ReformaTributaria.Infra.IoC
{
    public static class ContainerAuthentication
    {
        public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.SaveToken = true;

                 var privateKey = configuration.GetSection("AppConfig:PrivateKey").Value;

                 if (string.IsNullOrEmpty(privateKey))
                     throw new InvalidOperationException("JWT PrivateKey configuration value is missing.");

                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(privateKey)),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });

            return services;
        }
    }
}
