namespace my_virtual_pets_api.Cloud;
public interface IStorageService
{
    Task<(bool, string)> UploadFileAsync(string keyName);
    Task UploadObject(byte[] imgToUpload, string key);
}