using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Repositories.Interfaces
{
    public interface IPetRepository
    {
        List<PetCardDataDTO> GetAllPetsByUserID(Guid userId);

        public PetCardDataDTO GetPetById(Guid petId);

        public PetCardDataDTO AddPet(AddPetDTO petData, Guid imageId);

    }
}
