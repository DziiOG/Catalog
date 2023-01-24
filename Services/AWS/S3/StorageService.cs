using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Catalog.Contracts;
using Catalog.Intallers.Services;
using Catalog.Settings;

namespace Catalog.Services.AWS.S3
{
    public class StorageService : IStorageService
    {
        public async Task<S3Response> UploadFileAsync(
            S3Object s3obj,
            AmazonClientSettings awsCredentials
        )
        {
            var credentials = new BasicAWSCredentials(
                awsCredentials.accessKeyId,
                awsCredentials.secretAccessKey
            );

            var config = new AmazonS3Config() { RegionEndpoint = Amazon.RegionEndpoint.USEast2 };

            var response = new S3Response();

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = s3obj.InputStream,
                    Key = s3obj.Name,
                    BucketName = s3obj.BucketName,
                    CannedACL = S3CannedACL.NoACL,
                };
                using var client = new AmazonS3Client(credentials, config);

                var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);

                response.StatusCode = 200;
                response.Message = $"{s3obj.Name} has been uploaded successfully";
            }
            catch (AmazonS3Exception ex)
            {
                response.StatusCode = (int)ex.StatusCode;
                response.Message = ex.Message;
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
            }

            return response;
        }
    }
}
