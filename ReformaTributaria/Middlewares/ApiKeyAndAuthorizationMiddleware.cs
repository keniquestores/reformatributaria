using System.Net;
using Newtonsoft.Json;
using ReformaTributaria.CrossCutting.Models;

namespace ReformaTributaria.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="next"></param>
    public class ApiKeyAndAuthorizationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="productService"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.TryGetValue("x-api-key", out var apiKey);

            if (string.IsNullOrEmpty(apiKey))
            {
                await ReturnResponse(context, HttpStatusCode.Unauthorized, "ApiKey inválida.");
                return;
            }

            await _next(context);

            return;
        }

        private static async Task ReturnResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            if (context.Response.HasStarted) return;

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new GenericResponse
            {
                Success = false,
                Data = default!,
                Messages = [message]
            };

            var json = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(json);
        }
    }
}
