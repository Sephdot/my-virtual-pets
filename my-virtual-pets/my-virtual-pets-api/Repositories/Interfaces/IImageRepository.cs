namespace my_virtual_pets_api.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<Guid> AddImage(string imageUrl);

    }
}