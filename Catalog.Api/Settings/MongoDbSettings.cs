namespace Catalog.Api.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        public string ConnectionString
        {
            get { return $"mongodb://{Host}:{Port}"; }
        }
    }
}
