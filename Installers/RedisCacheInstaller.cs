using Catalog.Interfaces.Installers;
using Catalog.Interfaces.Redis;
using Catalog.Services.Redis;
using Catalog.Settings;
using StackExchange.Redis;

namespace Catalog.Intallers
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
