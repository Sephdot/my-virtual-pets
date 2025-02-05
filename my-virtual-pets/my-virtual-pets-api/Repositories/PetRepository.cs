using Microsoft.AspNetCore.Mvc;
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
        public List<Pet> GetPets()
        {
            return _context.Pets.Include(p => p.Image).ToList();
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

        public PetCardDataDTO GetPetById(Guid petId)
        {
            return _context.Pets
                .Include(p => p.GlobalUser)
                .Include(p => p.Image)
                .Where(p => p.Id == petId)
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
                }).FirstOrDefault();
        }

        public PetCardDataDTO AddPet(AddPetDTO petData, Guid imageId)
        {
            Pet newPet = new Pet
            {
                ImageId = imageId,
                GlobalUserId = petData.OwnerId,
                Name = petData.PetName,
                Personality = petData.Personality,
                Type = petData.PetType,
                Description = petData.Description
            };
            _context.Pets.Add(newPet);
            _context.SaveChanges();
            Pet addedPet = _context.Pets.Include(p => p.GlobalUser)
                                        .Include(p => p.Image)
                                        .Single(p => p.Id == newPet.Id);

            PetCardDataDTO petCardDataDTO = Pet.CreatePetCardDto(addedPet);
            return petCardDataDTO;
        }
    }
}