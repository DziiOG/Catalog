using System.Text;
using System.Text.Json.Serialization;
using Catalog.Interfaces.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Catalog.Services.Redis
{
    public class RedisResponseCache : IRedisResponseCache
    {
        public RedisResponseCache() { }

        // public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration)
        // {
        //     if (response == null)
        //     {
        //         return;
        //     }

        //     string? serializedResponse = JsonConvert.SerializeObject(response);

        //     await _distributedCache.SetStringAsync(
        //         cacheKey,
        //         serializedResponse,
        //         new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = duration }
        //     );
        // }

        // public async Task<string?> GetCachedResponseAsync(string cacheKey)
        // {
        //     string? cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
        //     return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        // }

        public async Task<string?> GetCachedUserResponseAsync(string cacheKey)
        {
            var _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
            var db = _connectionMultiplexer.GetDatabase();
            var cachedResponse = await db.StringGetAsync(cacheKey);
            Console.WriteLine($"I was here {cacheKey}");
            // var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
            if (string.IsNullOrEmpty(cachedResponse))
            {
                Console.WriteLine("Was null");
                return null;
            }

            Console.WriteLine($"got here {cachedResponse}");
            // string stringResponse = Encoding.Default.GetString(cachedResponse);
            return string.IsNullOrEmpty("") ? null : "";
        }
    }
}
