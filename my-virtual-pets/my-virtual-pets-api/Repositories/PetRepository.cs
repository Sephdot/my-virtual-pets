using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly IDbContext _context;

        public PetRepository(IDbContext context)
        {
            _context = context;
        }

        public List<PetCardDataDTO> GetAllPetsByUserID(Guid userId)
        {
            return _context.Pets
                .Include(p => p.GlobalUser)
                .Include(p => p.Image)
                .Where(p => p.GlobalUserId == userId)
                .Select(p => new PetCardDataDTO
                {
                    PetId = p.Id,
                    PetName = p.Name,
                    ImageUrl = p.Image.ImageUrl,
                    OwnerUsername = p.GlobalUser.Username,
                    Score = 0,
                    Personality = p.Personality,
                    PetType = p.Type,
                    Description = p.Description,
                    IsFavourited = false
                })
                .ToList();
        }
    }
}