namespace Catalog.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";

        public const string Base = $"{Root}/{Version}";

        public static class CatalogRoutes
        {
            public const string CatalogBase = $"{Base}/items";
            public const string ById = "{id}";
        }
    }
}
