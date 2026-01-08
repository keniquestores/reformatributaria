using Microsoft.AspNetCore.Mvc;
using ReformaTributaria.Filters;

namespace ReformaTributaria.Controllers
{
    [ServiceFilter(typeof(ResultActionFilter))]
    public class QControllerBaseController : ControllerBase
    {
        protected ActionResult ResultJson(object? value = null)
        {
            return new JsonResult(value);
        }

        protected string? GetApiKeyFromHeader()
        {
            if (Request.Headers.TryGetValue("x-api-key", out var apiKey))
            {
                return apiKey.ToString();
            }

            return null;
        }

        protected string? GetQuestorIdFromClaims()
        {
            var questorIdClaim = User.FindFirst("QuestorId");

            if (questorIdClaim != null)
            {
                return questorIdClaim.Value;
            }

            return null;
        }

        protected bool? GetIsProductFromClaims()
        {
            var IsProductClaim = User.FindFirst("IsProduct");

            if (IsProductClaim != null)
            {
                if (bool.TryParse(IsProductClaim.Value, out var result))
                {
                    return result;
                }
                return null;
            }

            return null;
        }
    }
}
