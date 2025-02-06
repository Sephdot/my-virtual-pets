using my_virtual_pets_api.Entities;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IPetService
    {
        List<PetCardDataDTO> GetPetsByUser(Guid userId);
        public PetCardDataDTO GetPetById(Guid petId);
        public PetCardDataDTO AddPet(AddPetDTO addPetDTO);

        public bool DeletePet(Guid petId);
        public List<Pet> GetPets();
    }
}
