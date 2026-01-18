using Microsoft.AspNetCore.Mvc.Filters;
using Tradelens.Core.Interfaces;

namespace Tradelens.Api.RequestHelpers;

/// <summary>
/// Action filter attribute that invalidates an existing cached response(s) based on a common url pattern
/// Apply to controller actions.
/// </summary>
/// <param name="pattern">The pattern to be matched against all keys in the database</param>
[AttributeUsage(AttributeTargets.Method)]
public class InvalidateCacheAttribute(string pattern) : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// If the result was successful, i.e. a valid POST request, then remove all the cache keys that match the pattern.
    /// </summary>
    /// <param name="context">Provides access to the action execution context, including HTTP request details and services.</param>
    /// <param name="next">A delegate that invokes the next filter in the pipeline or the action method itself.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        
        if (resultContext.Exception == null || resultContext.ExceptionHandled)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            await cacheService.RemoveCacheByPattern(pattern);
        }
    }
}