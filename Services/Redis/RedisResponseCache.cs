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
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisResponseCache(ConnectionMultiplexer connectionMultiplexer)
        {
            this._connectionMultiplexer = connectionMultiplexer;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration)
        {
            if (response == null)
            {
                return;
            }

            string? serializedResponse = JsonConvert.SerializeObject(response);

            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(cacheKey, serializedResponse, duration);
        }

        public async Task<string?> GetCachedResponseAsync(string cacheKey)
        {
            var db = _connectionMultiplexer.GetDatabase();
            string? cachedResponse = await db.StringGetAsync(cacheKey);
            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}
