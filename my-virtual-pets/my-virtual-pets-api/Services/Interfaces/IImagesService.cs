using my_virtual_pets_class_library;

namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IImagesService
    {
        Task<ImagesResponseDto?> ProcessImageAsync(byte[] inputImage);
    }
}