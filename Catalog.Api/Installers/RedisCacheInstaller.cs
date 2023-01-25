using Catalog.Api.Interfaces.Installers;
using Catalog.Api.Interfaces.Redis;
using Catalog.Api.Services.Redis;
using Catalog.Api.Settings;
using StackExchange.Redis;

namespace Catalog.Api.Intallers
{
    public class RedisCacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.Bind(key: nameof(redisCacheSettings), redisCacheSettings);
            services.AddSingleton(redisCacheSettings);
            if (!redisCacheSettings.Enabled)
            {
                return;
            }
            var connectionMultiplexer = ConnectionMultiplexer.Connect(
                redisCacheSettings.ConnectionString
            );
            services.AddSingleton(connectionMultiplexer);

            services.AddSingleton<IRedisResponseCache, RedisResponseCache>();
        }
    }
}
