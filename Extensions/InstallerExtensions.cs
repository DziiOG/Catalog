using Catalog.Interfaces.Installers;

namespace Catalog.Extensions
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            List<IInstaller>? Installers = typeof(Program).Assembly.ExportedTypes
                .Where(
                    exportedType =>
                        typeof(IInstaller).IsAssignableFrom(exportedType)
                        && !exportedType.IsInterface
                        && !exportedType.IsAbstract
                )
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            Installers.ForEach(Installer => Installer.InstallServices(services, configuration));
        }
    }
}
