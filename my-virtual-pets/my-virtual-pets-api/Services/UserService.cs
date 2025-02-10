using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;
using System.Net.Mail;

namespace my_virtual_pets_api.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IPetService _petService;


        public UserService(IUserRepository userRepository, IPetService petService)
        {
            _userRepository = userRepository;
            _petService = petService;
        }


        public bool ExistsByUsername(string username)
        {
            return _userRepository.ExistsByUsername(username);
        }

        public bool ExistsByEmail(string email)
        {
            bool validFormat = IsValidEmail(email);
            if (!validFormat) throw new FormatException("Invalid email format.");
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
            var request = _userRepository.AddToFavourites(GlobalUserId, PetId);
            if (request)
            {
                _petService.IncreaseScore(PetId);
            }
            return request;
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
            var request = _userRepository.RemoveFromFavourites(GlobalUserId, PetId);
            if (request)
            {
                _petService.DecreaseScore(PetId);
            }
            return request;
        }

        public bool UpdateUser(UpdateUserDTO updatedUser, string currentPassword)
        {
            return _userRepository.UpdateUser(updatedUser, currentPassword);
        }

        public bool IsFavourited(Guid GlobalUserId, Guid PetId)
        {
            return _userRepository.IsFavourited(GlobalUserId, PetId);

        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var addr = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
