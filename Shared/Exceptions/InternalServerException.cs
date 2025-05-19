using Microsoft.AspNetCore.Http;

namespace Shared.Exceptions;

public class InternalServerException : CustomHttpException
{
    const int statusCode = StatusCodes.Status500InternalServerError;

    public InternalServerException(string message) : base(message, statusCode)
    {
        StatusCode = statusCode;
    }

    public InternalServerException(string message, string details) : base(message, statusCode)
    {
        Details = details;
        StatusCode = statusCode;
    }
}
