using Catalog.Api.Interfaces.Installers;
using Catalog.Api.Settings;

namespace Catalog.Api.Intallers
{
    public class BotSettingsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            ResquestBotSettings? resquestBotSettings = new ResquestBotSettings();
            configuration.Bind(key: nameof(resquestBotSettings), resquestBotSettings);
            services.AddSingleton(resquestBotSettings);
        }
    }
}
