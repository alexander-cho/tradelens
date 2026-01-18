using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tradelens.Core.Interfaces;

namespace Tradelens.Api.RequestHelpers;

/// <summary>
/// Action filter attribute that caches HTTP responses in Redis to improve performance.
/// Apply to controller actions or classes to cache their responses for a specified duration.
/// </summary>
/// <remarks>
/// allows us to add Action filter to action i.e. ActionResult
/// do something just before action is going to be executed; check if the cacheKey exists and then something right after
/// </remarks>
/// <param name="timeToLiveSeconds">The duration in seconds that the cached response remains valid before invalidating.</param>
[AttributeUsage(AttributeTargets.All)]
public class CacheAttribute(int timeToLiveSeconds) : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// Executes the caching logic before and after the action method.
    /// Checks Redis for a cached response; if found, returns it immediately (short-circuit).
    /// If not found, executes the action, then caches the result for future requests.
    /// </summary>
    /// <param name="context">Provides access to the action execution context, including HTTP request details and services.</param>
    /// <param name="next">A delegate that invokes the next filter in the pipeline or the action method itself.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cachedResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
            context.Result = contentResult;

            return;
        }

        var executedContext = await next();

        if (executedContext.Result is OkObjectResult okObjectResult)
        {
            if (okObjectResult.Value != null)
            {
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(timeToLiveSeconds));
            }
        }
    }

    /// <summary>
    /// Generates a unique cache key based on the request path and query parameters.
    /// Query parameters are ordered alphabetically to ensure consistent keys for identical requests.
    /// </summary>
    /// <param name="request">The HTTP request containing the path and query string parameters.</param>
    /// <returns>A formatted cache key string (e.g., "/api/cash-values|symbol=AAPL|date=2026-01-30").</returns>
    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"{request.Path}");
        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key}={value}");
        }

        return keyBuilder.ToString();
    }
}