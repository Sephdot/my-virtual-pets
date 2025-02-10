using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Entities;
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
        public async Task<List<Pet>> GetPets()
        {
            return await _context.Pets.Include(p => p.Image).ToListAsync();
        }
        public async Task<List<PetCardDataDTO>> GetAllPetsByUserID(Guid userId)
        {
            return await _context.Pets
                .Include(p => p.GlobalUser)
                .Include(p => p.Image)
                .Where(p => p.GlobalUserId == userId)
                .Select(p => new PetCardDataDTO
                {
                    PetId = p.Id,
                    PetName = p.Name,
                    ImageUrl = p.Image.ImageUrl,
                    OwnerUsername = p.GlobalUser.Username,
                    Score = p.Score,
                    Personality = p.Personality,
                    PetType = p.Type,
                    Description = p.Description,
                    IsFavourited = false
                })
                .ToListAsync();
        }

        public async Task<PetCardDataDTO?> GetPetById(Guid petId)
        {
            var pet = await _context.Pets
                .Include(p => p.GlobalUser)
                .Include(p => p.Image)
                .FirstOrDefaultAsync(p => p.Id == petId);

            if (pet == null)
            {
                return null;
            }

            return Pet.CreatePetCardDto(pet);
        }

        public async Task<PetCardDataDTO> AddPet(AddPetDTO petData, Guid imageId, int score)
        {
            Pet newPet = new Pet
            {
                ImageId = imageId,
                GlobalUserId = petData.OwnerId,
                Name = petData.PetName,
                Personality = petData.Personality,
                Type = petData.PetType,
                Description = petData.Description,
                DateCreated = DateTime.UtcNow,
                Score = score
            };
            _context.Pets.Add(newPet);
            await _context.SaveChangesAsync();
            Pet addedPet = await _context.Pets.Include(p => p.GlobalUser)
                                        .Include(p => p.Image)
                                        .SingleAsync(p => p.Id == newPet.Id);

            PetCardDataDTO petCardDataDTO = Pet.CreatePetCardDto(addedPet);
            return petCardDataDTO;
        }

        public async Task<bool> DeletePet(Guid id)
        {
            var petToRemove = await _context.Pets.Include(p => p.Image).SingleOrDefaultAsync(p => p.Id == id);
            if (petToRemove == null) return false;
            _context.Images.Remove(petToRemove.Image);
            _context.Pets.Remove(petToRemove);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<PetCardDataDTO>?> GetTop10Pets()
        {

            var pets = await _context.Pets
                .Include(p => p.GlobalUser)
                .Include(p => p.Image)
                .OrderByDescending(p => p.Score)
                .Take(10)
                .Select(p => Pet.CreatePetCardDto(p))
                .ToListAsync();

            if (pets.Count < 4) return null;

            return pets;
        }

        public async Task<List<PetCardDataDTO>?> GetRecentPets()
        {
            var pets = await _context.Pets
                .Include(p => p.GlobalUser)
                .Include(p => p.Image)
                .OrderByDescending(p => p.DateCreated)
                .Take(10)
                .Select(p => Pet.CreatePetCardDto(p))
                .ToListAsync();

            if (pets == null || pets.Count < 2)
            {
                return null;
            }

            return pets;
        }

        public async Task IncreaseScore(Guid petId)
        {
            var pet = await _context.Pets
                .SingleOrDefaultAsync(p => p.Id == petId);
            if (pet == null) throw new KeyNotFoundException("Invalid pet Id.");
            pet.Score += 1;
            await _context.SaveChangesAsync();
        }


        public async Task DecreaseScore(Guid petId)
        {
            var pet = await _context.Pets
                .SingleOrDefaultAsync(p => p.Id == petId);
            if (pet == null) throw new KeyNotFoundException("Invalid pet Id");
            pet.Score -= 1;
            await _context.SaveChangesAsync();
        }
    }
}