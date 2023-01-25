using Catalog.Contracts;
using Catalog.Services.AWS.S3;
using Catalog.Settings;

namespace Catalog.Intallers.Services
{
    public interface IStorageService
    {
        Task<S3Response> UploadFileAsync(
            S3Object s3obj,
            AmazonClientSettings awsCredentials,
            string ContentType
        );
    }
}
