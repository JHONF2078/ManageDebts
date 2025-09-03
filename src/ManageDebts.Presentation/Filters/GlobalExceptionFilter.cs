using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Authentication;
using ManageDebts.Application.Common.Exceptions;

namespace ManageDebts.Presentation.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            ProblemDetails problemDetails;
            int statusCode;

            switch (context.Exception)
            {
                case NotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    problemDetails = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = "Resource not found.",
                        Detail = notFoundEx.Message
                    };
                    break;
                case KeyNotFoundException keyNotFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    problemDetails = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = "Resource not found.",
                        Detail = keyNotFoundEx.Message
                    };
                    break;
                case AuthenticationException authEx:
                case UnauthorizedAccessException unauthEx:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    problemDetails = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = "Unauthorized access.",
                        Detail = context.Exception.Message
                    };
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    problemDetails = new ProblemDetails
                    {
                        Status = statusCode,
                        Title = "An unexpected error occurred.",
                        Detail = context.Exception.Message
                    };
                    break;
            }

            _logger.LogError(context.Exception, "Exception handled by GlobalExceptionFilter");
            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }
    }
}
