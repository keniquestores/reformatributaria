using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace ReformaTributaria.Handlers
{
    public sealed class CustomAuthorizationResultHandler
     : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            if (!authorizeResult.Succeeded)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "ProductContextRequired",
                    message = "Para acessar este recurso, é necessário estar autenticado como um produto."
                });

                return;
            }

            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }

}
