using Domain.Exceptions;
using Shared.ErrorModels;
using System.ComponentModel.DataAnnotations;

namespace Web.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Handle invalid endpoints (404)
                if (context.Response.StatusCode == StatusCodes.Status404NotFound &&
                    !context.Response.HasStarted)
                {
                    await HandleNotFoundEndpointAsync(context);
                }
            }
            catch (DomainValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (AppException ex)
            {
                await HandleAppExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleUnhandledExceptionAsync(context, ex);
            }
        }


        private async Task HandleAppExceptionAsync(HttpContext context, AppException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            context.Response.Clear();
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                StatusCode = ex.StatusCode,
                ErrorMassage = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private async Task HandleValidationExceptionAsync(
            HttpContext context,
            DomainValidationException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMassage = "Validation Error",
                Errors = ex.Errors
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private async Task HandleUnhandledExceptionAsync(
            HttpContext context,
            Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorMassage = "An unexpected error occurred"
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private async Task HandleNotFoundEndpointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMassage = $"Endpoint '{context.Request.Path}' not found"
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
