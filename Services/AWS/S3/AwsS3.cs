namespace Catalog.Services.AWS.S3
{
    public class S3Object
    {
        public string Name { get; set; } = string.Empty;
        public MemoryStream InputStream { get; set; } = null!;
        public string BucketName { get; set; } = null!;
    }
}
