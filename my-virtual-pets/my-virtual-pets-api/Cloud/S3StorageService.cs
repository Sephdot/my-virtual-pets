using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace my_virtual_pets_api.Cloud
{
    public class S3StorageService
    {
        private TransferUtility _fileTransferUtility;

        private string filePath = "C:\\Users\\josep\\Documents\\Northcoders\\projects\\my-virtual-pets\\my-virtual-pets\\my-virtual-pets-api\\Resources\\Images\\testimage.png";
        private const string BUCKET_NAME = "my-virtual-pets-images";
        private const string KEY_NAME = "dogyesyes";
        private const string URI_BEGINNING = "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/";
        private const string ACCESS_KEY = "";
        private const string SECRET_KEY = "";

        public S3StorageService()
        {
            BasicAWSCredentials credentials = new(ACCESS_KEY, SECRET_KEY);
            AmazonS3Client s3Client = new(credentials, Amazon.RegionEndpoint.EUWest2);
            _fileTransferUtility = new(s3Client);
        }

        public async Task<string> UploadFileAsync()
        {
            try
            {
                await _fileTransferUtility.UploadAsync(filePath, BUCKET_NAME, KEY_NAME);
                return URI_BEGINNING + KEY_NAME;
                
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("AWS Error: " + e.Message);
                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }
    }
}
