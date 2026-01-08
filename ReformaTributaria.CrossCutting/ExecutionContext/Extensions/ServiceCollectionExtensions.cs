using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReformaTributaria.CrossCutting.EFCore;
using ReformaTributaria.CrossCutting.ExecutionContext.Abstractions;
using ReformaTributaria.CrossCutting.ExecutionContext.Implementation;
using ReformaTributaria.CrossCutting.Notifications;
using ReformaTributaria.CrossCutting.Validation;

namespace ReformaTributaria.CrossCutting.ExecutionContext.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestScope(
       this IServiceCollection services)
        {
            services.AddScoped<IRequestScope, RequestScope>();
            services.AddScoped<INotificationHandler, NotificationHandler>();
            services.AddScoped<IValidatorHandler, ValidatorHandler>();

            return services;
        }

        public static IServiceCollection AddRequestScopeEfCore<TContext>(
            this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<ITransactionalContext, EFTransactionalContext<TContext>>();
            return services;
        }
    }
}
