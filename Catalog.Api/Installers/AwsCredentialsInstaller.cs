using Catalog.Api.Intallers.Services;
using Catalog.Api.Interfaces.Installers;
using Catalog.Api.Services.AWS.S3;
using Catalog.Api.Settings;

namespace Catalog.Api.Intallers
{
    public class AwsCredentialsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            AmazonClientSettings? awsCredentials = new AmazonClientSettings();
            configuration.Bind(key: nameof(awsCredentials), awsCredentials);
            services.AddSingleton(awsCredentials);
            services.AddSingleton<IStorageService, StorageService>();
        }
    }
}
