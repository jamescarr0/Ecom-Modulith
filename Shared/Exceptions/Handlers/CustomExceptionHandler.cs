using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Shared.Exceptions.Handlers;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is CustomHttpException customHttpException)
        {
            await customHttpException.HandleAsync(httpContext, logger, cancellationToken: cancellationToken);
            return true;
        }

        // Fluent Validation Model Errors.
        if (exception is ValidationException validationException)
        {
            var validationErrors = validationException.Errors;

            var validationErrorsExtension = new Dictionary<string, object?>
            {
                { "validationErrors", validationErrors},
            };

            var badRequestException = new BadRequestException("Validation Failed");
            await badRequestException.HandleAsync(httpContext, logger, validationErrorsExtension, cancellationToken: cancellationToken);

            return true;
        }

        var exceptionType = exception.GetType().Name;

        logger.LogError("Unexpected exception ({exceptionType}): traceId: {traceId}, message: {message}, time: {time}",
            GetType().Name,
            httpContext.TraceIdentifier,
            exception.Message,
            DateTime.UtcNow);

        return false;
    }
}
