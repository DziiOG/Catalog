using Catalog.Interfaces.Installers;
using Catalog.Settings;

namespace Catalog.Intallers
{
    public class MongoDBInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<MongoDB.Driver.IMongoClient>(serviceProvide =>
            {
                MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(
                    new MongoDB.Bson.Serialization.Serializers.GuidSerializer(
                        MongoDB.Bson.BsonType.String
                    )
                );
                MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(
                    new MongoDB.Bson.Serialization.Serializers.DateTimeOffsetSerializer(
                        MongoDB.Bson.BsonType.String
                    )
                );
                MongoDbSettings? mongoDbSettings = new MongoDbSettings();

                configuration.Bind(key: nameof(mongoDbSettings), mongoDbSettings);

                string host = mongoDbSettings.Host;
                int port = mongoDbSettings.Port;

                Catalog.Settings.MongoDbSettings settings = new Catalog.Settings.MongoDbSettings()
                {
                    Host = host,
                    Port = port
                };
                return new MongoDB.Driver.MongoClient(settings.ConnectionString);
            });
        }
    }
}
