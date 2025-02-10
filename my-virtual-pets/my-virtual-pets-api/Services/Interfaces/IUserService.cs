using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Services.Interfaces;

public interface IUserService
{
    public Task<bool> ExistsByUsername(string username);
    public Task CreateNewLocalUser(NewUserDTO newUserDto);
    public Task<bool> DoesPasswordMatch(UserLoginDTO userLoginDto);
    public Task<bool> ExistsByEmail(string email);
    public Task<Guid> GetUserIdByUsername(string username);

    public Task<UserDisplayDTO> GetUserDetailsByUserId(Guid userId);

    public Task<bool> AddToFavourites(Guid GlobalUserId, Guid PetId);

    public Task<List<Guid>> GetFavouritePetId(Guid GlobalUserId);
    public Task<List<PetCardDataDTO>> GetFavouritePets(Guid GlobalUserId);
    public Task<bool> RemoveFromFavourites(Guid GlobalUserId, Guid PetId);
    public Task<bool> IsFavourited(Guid GlobalUserId, Guid PetId);

    public Task<Guid> CreateNewAuthUser(string email, string fullname, string authid);
    public Task<bool> UpdateUser(UpdateUserDTO updatedUser, string currentPassword);


}