using Microsoft.Extensions.Localization;

namespace Catalog.Api.Interfaces.Redis
{
    public interface IRedisResponseCache
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration);

        Task<string?> GetCachedResponseAsync(string cacheKey);
    }
}
