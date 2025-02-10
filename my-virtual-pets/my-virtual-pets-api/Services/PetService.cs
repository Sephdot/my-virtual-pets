using my_virtual_pets_api.Entities;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;
using my_virtual_pets_class_library.Enums;

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
        public async Task<List<Pet>> GetPets()
        {
            return await _petRepository.GetPets();
        }
        public async Task<List<PetCardDataDTO>> GetPetsByUser(Guid userId)
        {
            return await _petRepository.GetAllPetsByUserID(userId);
        }

        public async Task<PetCardDataDTO?> GetPetById(Guid petId)
        {
            return await _petRepository.GetPetById(petId);
        }

        public async Task<PetCardDataDTO> AddPet(AddPetDTO petData)
        {
            Guid imageId = _imagesService.AddImage(petData.ImageUrl);
            int score = GenerateScore(petData);
            return await _petRepository.AddPet(petData, imageId, score);
        }

        private int GenerateScore(AddPetDTO pet)
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
        public async Task IncreaseScore(Guid petId)
        {
            await _petRepository.IncreaseScore(petId);
        }
        public async Task DecreaseScore(Guid petId)
        {
            await _petRepository.DecreaseScore(petId);
        }

        public async Task<List<PetCardDataDTO>?> GetTop10Pets()
        {
            return await _petRepository.GetTop10Pets();
        }

        public async Task<List<PetCardDataDTO>?> GetRecentPets()
        {
            return await _petRepository.GetRecentPets();
        }

        public async Task<bool> DeletePet(Guid petId)
        {
            return await _petRepository.DeletePet(petId);
        }

    }
}
