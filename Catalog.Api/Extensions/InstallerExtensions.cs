using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Api.Interfaces.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.Extensions
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
