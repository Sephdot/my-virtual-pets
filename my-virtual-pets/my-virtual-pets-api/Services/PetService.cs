using my_virtual_pets_api.Entities;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;

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
            return _petRepository.AddPet(petData, imageId);
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
