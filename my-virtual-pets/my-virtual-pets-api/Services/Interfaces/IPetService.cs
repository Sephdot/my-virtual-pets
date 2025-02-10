using my_virtual_pets_api.Entities;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IPetService
    {
        Task<List<PetCardDataDTO>> GetPetsByUser(Guid userId);
        Task<PetCardDataDTO?> GetPetById(Guid petId);
        Task<PetCardDataDTO> AddPet(AddPetDTO addPetDTO);
        Task<bool> DeletePet(Guid petId);
        Task<List<Pet>> GetPets();
        Task<List<PetCardDataDTO>?> GetTop10Pets();
        Task<List<PetCardDataDTO>?> GetRecentPets();
        Task IncreaseScore(Guid petId);
        Task DecreaseScore(Guid petId);
    }
}
