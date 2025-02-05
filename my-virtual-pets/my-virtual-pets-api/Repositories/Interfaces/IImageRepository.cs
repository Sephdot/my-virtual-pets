namespace my_virtual_pets_api.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Guid AddImage(string imageUrl);
    }
}