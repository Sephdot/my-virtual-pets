using Microsoft.Extensions.Hosting;
using my_virtual_pets_api.Entities;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;
using my_virtual_pets_class_library.Enums;
using System.Diagnostics;

namespace my_virtual_pets_api.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IImagesService _imagesService;


        public PetService(IPetRepository petRepository, IImagesService imagesService)
        {
            _petRepository = petRepository;
            _imagesService = imagesService;
        }
        public List<Pet> GetPets()
        {
            return _petRepository.GetPets();
        }
        public List<PetCardDataDTO> GetPetsByUser(Guid userId)
        {
            return _petRepository.GetAllPetsByUserID(userId);
        }

        public PetCardDataDTO GetPetById(Guid petId)
        {
            return _petRepository.GetPetById(petId);
        }

        public PetCardDataDTO AddPet(AddPetDTO petData)
        {
            Guid imageId = _imagesService.AddImage(petData.ImageUrl);
            int score = GenerateScore(petData);
            return _petRepository.AddPet(petData, imageId, score);
        }

        public int GenerateScore(AddPetDTO pet)
        {
            Random rnd = new Random();
            int roll1 = rnd.Next(90);
            int roll2 = rnd.Next(90);
            int roll3 = rnd.Next(90);
            float baseScore = (roll1 + roll2 + roll3) / 3;

            switch (pet.Personality)
            {
                case Personality.BOLD:
                case Personality.BRAVE:
                    baseScore *= 1.1f;
                    break;
                case Personality.TIMID:
                case Personality.QUIET:
                    baseScore *= 0.95f;
                    break;
                case Personality.NAUGHTY:
                case Personality.HASTY:
                    baseScore *= 0.9f;
                    break;
            }

            if (String.IsNullOrEmpty(pet.Description)) baseScore *= 0.8f;

            return ((int)baseScore);

        }
        public void IncreaseScore(Guid petId)
        {
            _petRepository.IncreaseScore(petId);
        }

        public List<PetCardDataDTO> GetTop10Pets()
        {
            return _petRepository.GetTop10Pets();
        }

        public List<PetCardDataDTO> GetRecentPets()
        {
            return _petRepository.GetRecentPets();
        }

        public bool DeletePet(Guid petId)
        {
            return _petRepository.DeletePet(petId);
        }
        
    }
}
