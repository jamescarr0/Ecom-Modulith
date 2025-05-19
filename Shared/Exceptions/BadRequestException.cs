using Microsoft.AspNetCore.Http;

namespace Shared.Exceptions;

public class BadRequestException : CustomHttpException
{
    const int statusCode = StatusCodes.Status400BadRequest;

    public BadRequestException(string message) : base(message, statusCode)
    {
    }

    public BadRequestException(string message, string details) : base(message, statusCode)
    {
        Details = details;
    }
}
