using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace my_virtual_pets_api.Cloud
{
    public class S3StorageService : IStorageService
    {
        private TransferUtility _fileTransferUtility;
        private AmazonS3Client _s3Client;

        private const string BUCKET_NAME = "my-virtual-pets-images";
        private const string URI_BEGINNING = "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/";
        private string _accessKey;
        private string _secretKey;

        public S3StorageService()
        {
            _accessKey = Environment.GetEnvironmentVariable("ACCESS_KEY__my_virtual_pets") ?? throw new Exception("Access key cannot be null");
            _secretKey = Environment.GetEnvironmentVariable("SECRET_KEY__my_virtual_pets") ?? throw new Exception("Secret key cannot be null");
            BasicAWSCredentials credentials = new(_accessKey, _secretKey);
            _s3Client = new(credentials, Amazon.RegionEndpoint.EUWest2);
            _fileTransferUtility = new(_s3Client);
        }

        public async Task<(bool isSuccess, string imageUrl)> UploadObjectAsync(byte[] imgToUpload, string key)
        {
            var request = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = BUCKET_NAME,
                Key = $"{key}.png",
                ContentType = "image/png"
            };
            try
            {
                using (var ms = new MemoryStream(imgToUpload))
                {
                    request.InputStream = ms;
                    await _s3Client.PutObjectAsync(request);
                }
                return (true, URI_BEGINNING + key + ".png");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"AWS error:\n{e.Message}");
                return (false, e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return (false, e.Message);
            }

        }
    }
}
