namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IImagesService
    {
        Task<byte[]?> ProcessImageAsync(byte[] inputImage);
    }
}