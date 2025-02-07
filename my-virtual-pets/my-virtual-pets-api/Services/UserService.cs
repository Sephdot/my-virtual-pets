using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public bool ExistsByUsername(string username)
        {
            return _userRepository.ExistsByUsername(username);
        }

        public bool ExistsByEmail(string email)
        {
            return _userRepository.ExistsByEmail(email);
        }


        public void CreateNewLocalUser(NewUserDTO newUserDto)
        {
            newUserDto.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newUserDto.Password);
            Guid globalUserId = _userRepository.CreateNewGlobalUser(newUserDto);
            _userRepository.CreateNewLocalUser(newUserDto, globalUserId);
        }

        public bool DoesPasswordMatch(UserLoginDTO userLoginDto)
        {
            string hashedPassword = _userRepository.GetPassword(userLoginDto.Username);
            return BCrypt.Net.BCrypt.EnhancedVerify(userLoginDto.Password, hashedPassword);
        }

        public Guid GetUserIdByUsername(string username)
        {
            return _userRepository.GetUserIdByUsername(username);
        }

        public UserDisplayDTO GetUserDetailsByUserId(Guid userId)
        {
            return _userRepository.GetUserDetailsByUserId(userId);
        }

        public bool AddToFavourites(Guid GlobalUserId, Guid PetId)
        {
            return _userRepository.AddToFavourites(GlobalUserId, PetId);
        }

        public List<Guid> GetFavouritePetId(Guid GlobalUserId)
        {
            return _userRepository.GetFavoritePetIds(GlobalUserId);
        }

        public List<PetCardDataDTO> GetFavouritePets(Guid GlobalUserId)
        {
            return _userRepository.GetFavoritePetCards(GlobalUserId);
        }

        public bool RemoveFromFavourites(Guid GlobalUserId, Guid PetId)
        {
            return _userRepository.RemoveFromFavourites(GlobalUserId, PetId);
        }

        public bool IsFavourited(Guid GlobalUserId, Guid PetId)
        {
            return _userRepository.IsFavourited(GlobalUserId, PetId);
        }


    }
}
