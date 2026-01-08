using ReformaTributaria.Middlewares;

namespace IdentityServer.API.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiKeyAndAuthorizationMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiKey(
          this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyAndAuthorizationMiddleware>();
        }
    }
}
