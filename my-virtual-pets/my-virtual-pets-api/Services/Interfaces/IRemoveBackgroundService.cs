namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IRemoveBackgroundService
    {
        Task<byte[]?> RemoveBackgroundAsync(byte[] inputImage);
    }
}