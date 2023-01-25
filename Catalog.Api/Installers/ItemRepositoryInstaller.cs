using Catalog.Api.Interfaces.Installers;
using Catalog.Api.Interfaces.Repositories;
using Catalog.Api.Repositories;

namespace Catalog.Api.Intallers
{
    public class ItemRepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IItemRepository, MongoDbItemRepository>();
        }
    }
}
