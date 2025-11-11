using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Log the error
            _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

            // Pattern matching to determine exception type
            var (statusCode, title, details) = exception switch
            {
                ValidationException validationEx => (
                    (int)HttpStatusCode.BadRequest,
                    "Validation error",
                    validationEx.Message
                ),

                ArgumentException argEx => (
                    (int)HttpStatusCode.BadRequest,
                    "Invalid argument",
                    argEx.Message
                ),

                KeyNotFoundException notFoundEx => (
                    (int)HttpStatusCode.NotFound,
                    "Resource not found",
                    notFoundEx.Message
                ),

                UnauthorizedAccessException unauthorizedEx => (
                    (int)HttpStatusCode.Unauthorized,
                    "Unauthorized",
                    unauthorizedEx.Message
                ),

                _ => (
                    (int)HttpStatusCode.InternalServerError,
                    "Internal Server Error",
                    exception.Message
                )
            };

            // Create ProblemDetails response
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = details,
                Instance = context.Request.Path
            };

            // Add any extra info
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            // If it’s a FluentValidation exception, include validation errors
            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions["validationErrors"] =
                    validationException.Errors.Select(e => new
                    {
                        e.PropertyName,
                        e.ErrorMessage
                    });
            }

            // Write JSON response
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Exception was handled
        }
    }
}
