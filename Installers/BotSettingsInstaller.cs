using Catalog.Interfaces.Installers;
using Catalog.Settings;

namespace Catalog.Intallers
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
