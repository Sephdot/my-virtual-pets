using my_virtual_pets_api.Entities;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Repositories.Interfaces
{
    public interface IPetRepository
    {
        List<PetCardDataDTO> GetAllPetsByUserID(Guid userId);

        public PetCardDataDTO GetPetById(Guid petId);

        public PetCardDataDTO AddPet(AddPetDTO petData, Guid imageId);

        public bool DeletePet(Guid petId);
      
        public List<Pet> GetPets();

        public List<PetCardDataDTO> GetTop10Pets();
        public List<PetCardDataDTO> GetRecentPets();




    }
}
