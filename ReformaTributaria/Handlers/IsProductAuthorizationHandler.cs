using Microsoft.AspNetCore.Authorization;

namespace ReformaTributaria.Handlers
{
    public sealed class IsProductAuthorizationHandler
      : AuthorizationHandler<IsProductRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsProductRequirement requirement)
        {
            var isProductClaim = context.User.FindFirst("IsProduct");

            if (isProductClaim is not null &&
                bool.TryParse(isProductClaim.Value, out var isProduct) &&
                isProduct)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
