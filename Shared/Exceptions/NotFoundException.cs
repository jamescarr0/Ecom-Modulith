using Microsoft.AspNetCore.Http;

namespace Shared.Exceptions;

public class NotFoundException : CustomHttpException
{
    const int statusCode = StatusCodes.Status404NotFound;

    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found", statusCode)
    {
    }
}
