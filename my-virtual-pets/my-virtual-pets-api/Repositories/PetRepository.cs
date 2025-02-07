using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Entities;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_class_library.DTO;
using my_virtual_pets_class_library.Enums;

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
                    Score = p.Score,
                    Personality = p.Personality,
                    PetType = p.Type,
                    Description = p.Description,
                    IsFavourited = false
                })
                .ToList();
        }

        public PetCardDataDTO? GetPetById(Guid petId)
        {
            var pet = _context.Pets
                .Include(p => p.GlobalUser)
                .Include(p => p.Image)
                .FirstOrDefault(p => p.Id == petId);

            if (pet == null)
            {
                return null;
            }

            return new PetCardDataDTO
            {
                PetId = pet.Id,
                PetName = pet.Name,
                ImageUrl = pet.Image?.ImageUrl,
                OwnerUsername = pet.GlobalUser?.Username,
                Score = pet.Score,
                Personality = pet.Personality,
                PetType = pet.Type,
                Description = pet.Description,
                IsFavourited = false
            };
        }

        public PetCardDataDTO AddPet(AddPetDTO petData, Guid imageId, int score)
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
            _context.SaveChanges();
            Pet addedPet = _context.Pets.Include(p => p.GlobalUser)
                                        .Include(p => p.Image)
                                        .Single(p => p.Id == newPet.Id);

            PetCardDataDTO petCardDataDTO = Pet.CreatePetCardDto(addedPet);
            return petCardDataDTO;
        }

        public bool DeletePet(Guid id)
        {
            var petToRemove = _context.Pets.Include(p => p.Image).SingleOrDefault(p => p.Id == id);
            if (petToRemove == null) return false;
            _context.Images.Remove(petToRemove.Image);
            _context.Pets.Remove(petToRemove);
            _context.SaveChanges();
            return true;
        }


        public List<PetCardDataDTO> GetTop10Pets()
        {
        
            var pets = _context.Pets
                .Include(p => p.GlobalUser)
                .Include(p => p.Image)
                .AsEnumerable()  
                .OrderByDescending(p => p.Score)  
                .Take(10)
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
                .ToList();
                

            if (pets == null )
            {
                return null; 
            }

            return pets;
        }

        public List<PetCardDataDTO> GetRecentPets()
        {
            //TO DO: OrderByDescending creation date
            var pets = _context.Pets
                .Include(p => p.GlobalUser)
                .Include(p => p.Image)
                .AsEnumerable()
                .OrderByDescending(p => p.DateCreated)
                .Take(10)
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
                .ToList();


            if (pets == null)
            {
                return null;
            }

            return pets;
        }


    }
}