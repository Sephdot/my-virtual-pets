using my_virtual_pets_api.Data;
using my_virtual_pets_api.Entities;
using my_virtual_pets_api.Repositories.Interfaces;

namespace my_virtual_pets_api.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly IDbContext _context;

    public ImageRepository(IDbContext context)
    {
        _context = context;
    }

    public Guid AddImage(string imageUrl)
    {
        Image image = new Image { ImageUrl = imageUrl };
        _context.Images.Add(image);
        _context.SaveChanges();
        return image.Id;
    }
}