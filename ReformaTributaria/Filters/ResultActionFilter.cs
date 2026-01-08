using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReformaTributaria.CrossCutting.ExecutionContext.Abstractions;
using ReformaTributaria.CrossCutting.ExecutionContext.Exceptions;
using ReformaTributaria.CrossCutting.Models;
using ReformaTributaria.CrossCutting.Notifications;
using ReformaTributaria.CrossCutting.Validation;

namespace ReformaTributaria.Filters
{
    public sealed class ResultActionFilter(
        IRequestScope requestScope,
        INotificationHandler notificationHandler,
        ILogger<ResultActionFilter> logger,
        IValidatorHandler validatorHandler) : IActionFilter, IExceptionFilter
    {
        private readonly IRequestScope _requestScope = requestScope;
        private readonly INotificationHandler _notificationHandler = notificationHandler;
        private readonly ILogger<ResultActionFilter> _logger = logger;
        private readonly IValidatorHandler _validatorHandler = validatorHandler;

        private async Task<GenericResponse> CreateResultDefaultAsync(object? value)
        {
            var success = !_notificationHandler.HasNotificationsErrors();

            var messages = _notificationHandler
                .GetNotifications()
                .Select(x => x.Message)
                .ToList();

            _notificationHandler.DisposeNotifications();

            if (!success)
            {
                if (_validatorHandler.Commit())
                    await _requestScope.CompleteAsync();
                else
                    await _requestScope.FailAsync();
            }
            else
            {
                await _requestScope.CompleteAsync();
            }

            object? data = value is JsonResult json ? json.Value : value;

            return new GenericResponse
            {
                Success = success,
                Data = data ?? new object(),
                Messages = messages
            };
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
                return;

            context.Result = new JsonResult(
                CreateResultDefaultAsync(context.Result).GetAwaiter().GetResult()
            );
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Nada aqui
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidatorException)
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;

                context.Result = new JsonResult(
                    CreateResultDefaultAsync(context.Result).GetAwaiter().GetResult()
                );
                return;
            }

            _logger.LogError(context.Exception, "An unhandled exception occurred.");
        }
    }
}
