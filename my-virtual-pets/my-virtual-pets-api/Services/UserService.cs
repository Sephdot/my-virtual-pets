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


        public async Task<bool> ExistsByUsername(string username)
        {
            return await _userRepository.ExistsByUsername(username);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            bool validFormat = IsValidEmail(email);
            if (!validFormat) throw new FormatException("Invalid email format.");
            return await _userRepository.ExistsByEmail(email);
        }

        public async Task CreateNewLocalUser(NewUserDTO newUserDto)
        {
            newUserDto.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newUserDto.Password);
            Guid globalUserId = await _userRepository.CreateNewGlobalUser(newUserDto);
            await _userRepository.CreateNewLocalUser(newUserDto, globalUserId);
        }


        public async Task<bool> DoesPasswordMatch(UserLoginDTO userLoginDto)
        {
            string hashedPassword = await _userRepository.GetPassword(userLoginDto.Username);
            return BCrypt.Net.BCrypt.EnhancedVerify(userLoginDto.Password, hashedPassword);
        }

        public async Task<Guid> GetUserIdByUsername(string username)
        {
            return await _userRepository.GetUserIdByUsername(username);
        }

        public async Task<UserDisplayDTO> GetUserDetailsByUserId(Guid userId)
        {
            return await _userRepository.GetUserDetailsByUserId(userId);
        }

        public async Task<bool> AddToFavourites(Guid GlobalUserId, Guid PetId)
        {
            var request = await _userRepository.AddToFavourites(GlobalUserId, PetId);
            if (request)
            {
                await _petService.IncreaseScore(PetId);
            }
            return request;
        }

        public async Task<List<Guid>> GetFavouritePetId(Guid GlobalUserId)
        {
            return await _userRepository.GetFavoritePetIds(GlobalUserId);
        }

        public async Task<List<PetCardDataDTO>> GetFavouritePets(Guid GlobalUserId)
        {
            return await _userRepository.GetFavoritePetCards(GlobalUserId);
        }

        public async Task<bool> RemoveFromFavourites(Guid GlobalUserId, Guid PetId)
        {

            var request = await _userRepository.RemoveFromFavourites(GlobalUserId, PetId);
            if (request)
            {
                await _petService.DecreaseScore(PetId);
            }
            return request;
        }

        public async Task<bool> UpdateUser(UpdateUserDTO updatedUser, string currentPassword)
        {
            return await _userRepository.UpdateUser(updatedUser, currentPassword);
        }

        public async Task<bool> IsFavourited(Guid GlobalUserId, Guid PetId)
        {
            return await _userRepository.IsFavourited(GlobalUserId, PetId);

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
        
                
        public async Task<Guid> CreateNewAuthUser(string email, string fullname, string authid)
        {
            Guid globalUserId; 
            if (! await _userRepository.ExistsByEmail(email))
            {
                globalUserId = await _userRepository.CreateNewGlobalUser(email);
                _userRepository.CreateNewAuthUser(fullname, authid, globalUserId);
            } else
            {
                globalUserId = await _userRepository.GetUserIdByEmail(email);
            }
            return globalUserId;
        }
        

    }
}
