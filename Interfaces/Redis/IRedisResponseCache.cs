using Microsoft.Extensions.Localization;

namespace Catalog.Interfaces.Redis
{
    public interface IRedisResponseCache
    {
        // Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration);

        // Task<string?> GetCachedResponseAsync(string cacheKey);
        Task<string?> GetCachedUserResponseAsync(string cacheKey);
    }
}
