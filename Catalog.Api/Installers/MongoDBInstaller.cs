using Catalog.Api.Interfaces.Installers;
using Catalog.Api.Settings;

namespace Catalog.Api.Intallers
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

                MongoDbSettings settings = new MongoDbSettings() { Host = host, Port = port };
                return new MongoDB.Driver.MongoClient(settings.ConnectionString);
            });
        }
    }
}
