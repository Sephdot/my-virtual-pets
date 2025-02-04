using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public List<PetCardDataDTO> GetPetsByUser(Guid userId)
        {
            return _petRepository.GetAllPetsByUserID(userId);
        }
    }
}
