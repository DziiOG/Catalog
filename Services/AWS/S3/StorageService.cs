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
            AmazonClientSettings awsCredentials,
            string ContentType
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
                    ContentType = ContentType
                };

                using AmazonS3Client? client = new AmazonS3Client(credentials, config);

                TransferUtility? transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);

                var nowWOrk = response.StatusCode = 200;
                String[] spearator = { "completefarmer" };
                Int32 count = 2;
                String[] splittedElementArray = s3obj.BucketName.Split(
                    spearator,
                    count,
                    StringSplitOptions.RemoveEmptyEntries
                );
                response.Message = $"{s3obj.Name} has been uploaded successfully";
                response.Url =
                    $"https://completefarmer.s3.{awsCredentials.region}.amazonaws.com{splittedElementArray[0]}/"
                    + s3obj.Name;
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
