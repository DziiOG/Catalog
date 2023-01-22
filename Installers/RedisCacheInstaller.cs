using Catalog.Interfaces.Installers;
using Catalog.Interfaces.Redis;
using Catalog.Services.Redis;
using Catalog.Settings;

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

            services.AddStackExchangeRedisCache(
                options => options.Configuration = redisCacheSettings.ConnectionString
            );

            services.AddSingleton<IRedisResponseCache, RedisResponseCache>();
        }
    }
}
