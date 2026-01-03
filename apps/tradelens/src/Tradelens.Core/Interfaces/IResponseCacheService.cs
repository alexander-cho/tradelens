namespace Tradelens.Core.Interfaces;

public interface IResponseCacheService
{
    // key to retrieve cached object
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
    Task<string?> GetCachedResponseAsync(string cacheKey);
    Task RemoveCacheByPattern(string pattern);
}