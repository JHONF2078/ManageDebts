using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using ManageDebts.Application.Common.Exceptions;

namespace ManageDebts.Presentation.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception handled by ExceptionMiddleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string title;
            string detail = exception.Message;

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    title = "Resource not found.";
                    break;
                case KeyNotFoundException keyNotFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    title = "Resource not found.";
                    break;
                case AuthenticationException authEx:
                case UnauthorizedAccessException unauthEx:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    title = "Unauthorized access.";
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    title = "An unexpected error occurred.";
                    break;
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
