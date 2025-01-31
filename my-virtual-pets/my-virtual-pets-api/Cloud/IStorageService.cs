
namespace my_virtual_pets_api.Cloud
{
    public interface IS3StorageService
    {
        Task<(bool, string)> UploadFileAsync(string keyName);
        Task UploadObject(byte[] imgToUpload, string key);
    }
}