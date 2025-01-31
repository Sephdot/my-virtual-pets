
namespace my_virtual_pets_api.Services
{
    public interface IImagesService
    {
        Task<byte[]?> DownloadImageAsync(string url);
        Task<byte[]?> RemoveBackground(byte[] inputImage);

        Task<string?> RemoveBackground(string inputImageUrl);
        Task<byte[]?> RemoveBackgroundAsync(string inputImageUrl);
    }
}