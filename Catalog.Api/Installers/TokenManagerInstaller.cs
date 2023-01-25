using Catalog.Api.Interfaces.Installers;
using Catalog.Api.Interfaces.TokenAuthorization;
using Catalog.Api.Services.TokenAuthentication;

namespace Catalog.Api.Intallers
{
    public class TokenManagerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ITokenManager, TokenManager>();
        }
    }
}
