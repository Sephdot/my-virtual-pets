using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Repositories.Interfaces;

public interface IUserRepository
{

    public Task<bool> ExistsByUsername(string username);
    public Task<Guid> CreateNewLocalUser(NewUserDTO newUserDto, Guid globalUserId);
    public Task<Guid> CreateNewAuthUser(NewUserDTO newUserDto, Guid globalUserId);
    public Task<string> GetPassword(string username);
    public Task<Guid> CreateNewGlobalUser(NewUserDTO newUserDto);
    public Task<bool> ExistsByEmail(string email);
    public Task<Guid> GetUserIdByUsername(string username);

    public Task<UserDisplayDTO> GetUserDetailsByUserId(Guid userId);

    public Task<bool> AddToFavourites(Guid GlobalUserId, Guid PetId);

    public Task<List<Guid>> GetFavoritePetIds(Guid GlobalUserId);
    public Task<List<PetCardDataDTO>> GetFavoritePetCards(Guid GlobalUserId);

    public Task<bool> RemoveFromFavourites(Guid GlobalUserId, Guid PetId);


    public Task<bool> UpdateUser(UpdateUserDTO updatedUser, string currentPassword);


    public Guid CreateNewGlobalUser(string email);
    
    public Task<Guid> CreateNewAuthUser(string fullname, string authId, Guid globalUserId);
    public Task<bool> IsFavourited(Guid GlobalUserId, Guid PetId);

    public Task<Guid> GetUserIdByEmail(string email);
}