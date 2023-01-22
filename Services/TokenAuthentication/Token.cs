namespace Catalog.Services.TokenAuthentication
{
    public class Token
    {
        public string Value { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}
