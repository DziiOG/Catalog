namespace Catalog.Contracts
{
    public class S3Response
    {
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
