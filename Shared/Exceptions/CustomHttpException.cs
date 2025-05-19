﻿using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Shared.Exceptions;

public abstract class CustomHttpException : Exception
{
    public string? Details { get; init; }
    public int StatusCode { get; init; }

    protected CustomHttpException(string message, int statusCode) : base(message) {
        StatusCode = statusCode;
    }

    protected CustomHttpException(string message, string details, int statusCode) : base(message)
    {
        Details = details;
        StatusCode = statusCode;
    }

    public virtual async Task HandleAsync(HttpContext httpContext, ILogger logger, IDictionary<string, object?>? extensions = default, CancellationToken cancellationToken = default)
    {
        var problemDetails = new ProblemDetails
        {
            Title = GetType().Name,
            Status = StatusCode,
            Detail = Details ?? "None",
            Instance = httpContext.Request.Path,
            Extensions = new Dictionary<string, object?>()
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if(extensions is not null && extensions.Count > 0)
        {
            foreach(var kvp in extensions)
            {
                problemDetails.Extensions[kvp.Key] = kvp.Value;
            }
        }

        logger.LogError("Exception: {details}", JsonSerializer.Serialize(problemDetails));

        httpContext.Response.StatusCode = StatusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
    }
}
