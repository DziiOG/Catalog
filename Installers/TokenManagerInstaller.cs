using Catalog.Interfaces.Installers;

namespace Catalog.Intallers
{
    public class TokenManagerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<
                Catalog.Interfaces.TokenAuthorization.ITokenManager,
                Catalog.TokenAuthentication.TokenManager
            >();
        }
    }
}
