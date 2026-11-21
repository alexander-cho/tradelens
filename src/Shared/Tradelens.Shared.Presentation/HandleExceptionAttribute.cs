using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tradelens.Shared.Presentation;

/// <summary>
/// Exception handling filter attribute
/// </summary>
/// <remarks>
/// ASP.NET Core in Action, 3rd Edition, Andrew Lock. Listing 22.8 (page 545)
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class HandleExceptionAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // build problem details object to return in the response
        var error = new ProblemDetails
        {
            Title = "An error occurred",
            Detail = context.Exception.Message,
            Status = 500,
            Type = "https://httpwg.org/specs/rfc9110.html#status.500"
        };

        // create ObjectResult to serialize ProblemDetails and to set response status code
        context.Result = new ObjectResult(error)
        {
            StatusCode = 500
        };

        // marks the exception as handled to prevent it propogating into the middleware pipeline
        context.ExceptionHandled = true;
    }
}