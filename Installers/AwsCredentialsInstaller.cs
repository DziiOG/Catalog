using Catalog.Interfaces.Installers;
using Catalog.Settings;

namespace Catalog.Intallers
{
    public class AwsCredentialsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            AmazonClientSettings? awsCredentials = new AmazonClientSettings();
            configuration.Bind(key: nameof(awsCredentials), awsCredentials);
            services.AddSingleton(awsCredentials);
        }
    }
}
