namespace Catalog.Api.Settings
{
    public class RedisCacheSettings
    {
        public bool Enabled { get; set; }

        public string ConnectionString { get; set; } = string.Empty;
    }
}
