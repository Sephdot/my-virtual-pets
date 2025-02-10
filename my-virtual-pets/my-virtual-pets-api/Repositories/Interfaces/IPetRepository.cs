using my_virtual_pets_api.Entities;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Repositories.Interfaces
{
    public interface IPetRepository
    {
        Task<List<PetCardDataDTO>> GetAllPetsByUserID(Guid userId);

        Task<PetCardDataDTO?> GetPetById(Guid petId);

        Task<PetCardDataDTO> AddPet(AddPetDTO petData, Guid imageId, int score);

        Task<bool> DeletePet(Guid petId);

        Task<List<Pet>> GetPets();

        Task<List<PetCardDataDTO>?> GetTop10Pets();
        Task<List<PetCardDataDTO>?> GetRecentPets();
        Task IncreaseScore(Guid petId);
      
        //Make this async
        public void DecreaseScore(Guid petId);
    }
}
