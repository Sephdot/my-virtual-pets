namespace my_virtual_pets_api.Cloud;
public interface IStorageService
{
    Task<(bool, string)> UploadFileAsync(string keyName);
    Task<(bool isSuccess, string imageUrl)> UploadObjectAsync(byte[] imgToUpload, string key);
}