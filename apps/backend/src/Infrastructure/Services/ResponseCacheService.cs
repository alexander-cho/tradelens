using System.Text.Json;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class ResponseCacheService(IConnectionMultiplexer redis) : IResponseCacheService
{
    private readonly IDatabase _database = redis.GetDatabase(1);
    
    // convert c# object to string
    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    {
        var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var serializedResponse = JsonSerializer.Serialize(response, options);
        await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
    }

    public async Task<string?> GetCachedResponseAsync(string cacheKey)
    {
        var cachedResponse = await _database.StringGetAsync(cacheKey);

        if (cachedResponse.IsNullOrEmpty)
        {
            return null;
        }

        return cachedResponse;
    }

    public Task RemoveCacheByPattern(string pattern)
    {
        throw new NotImplementedException();
    }
}