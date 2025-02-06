using my_virtual_pets_class_library.Enums;

namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IImagesService
    {
        Task<ImagesResponseDTO?> ProcessImageAsync(byte[] inputImage);
        Guid AddImage(string imageUrl);
    }
}