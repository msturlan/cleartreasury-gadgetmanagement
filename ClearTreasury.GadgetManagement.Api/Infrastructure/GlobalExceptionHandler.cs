using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ClearTreasury.GadgetManagement.Api.Infrastructure;

public class GlobalExceptionHandler(ProblemDetailsFactory detailsFactory)
    : IExceptionHandler
{
    private static JsonSerializerOptions SerializerOptions { get; } = new()
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var instance = default(string);
        var details = "An unexpected error occurred.";
        var status = StatusCodes.Status500InternalServerError;

        if (exception is DbUpdateConcurrencyException)
        {
            status = StatusCodes.Status412PreconditionFailed;
            details = "The resource has already been modified by another party.";
            instance = httpContext.Request.Path;
        }
        
        var pd = detailsFactory.CreateProblemDetails(
            httpContext,
            statusCode: status,
            detail: details,
            instance: instance);
        
        httpContext.Response.StatusCode = status;

        await httpContext.Response.WriteAsJsonAsync(pd,
            SerializerOptions, MediaTypeNames.Application.ProblemJson, cancellationToken);

        return true;
    }
}
