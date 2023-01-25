using Catalog.Api.Contracts;
using Catalog.Api.Services.AWS.S3;
using Catalog.Api.Settings;

namespace Catalog.Api.Intallers.Services
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
