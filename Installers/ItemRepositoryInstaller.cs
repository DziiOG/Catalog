using Catalog.Interfaces.Installers;

namespace Catalog.Intallers
{
    public class ItemRepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<
                Catalog.Interfaces.Repositories.IItemRepository,
                Catalog.Repositories.MongoDbItemRepository
            >();
        }
    }
}
