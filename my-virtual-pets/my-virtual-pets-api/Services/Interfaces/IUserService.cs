using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Services.Interfaces;

public interface IUserService
{
    public bool ExistsByUsername(string username);
    public void CreateNewLocalUser(NewUserDTO newUserDto);
    public bool DoesPasswordMatch(UserLoginDTO userLoginDto);
    public bool ExistsByEmail(string email);
    public Guid GetUserIdByUsername(string username);

    public UserDisplayDTO GetUserDetailsByUserId(Guid userId);

    public bool AddToFavourites(Guid GlobalUserId, Guid PetId);

    public List<Guid> GetFavouritePetId(Guid GlobalUserId);
    public List<PetCardDataDTO> GetFavouritePets(Guid GlobalUserId);
    public bool RemoveFromFavourites(Guid GlobalUserId, Guid PetId);
    public bool IsFavourited(Guid GlobalUserId, Guid PetId);

    public bool UpdateUser(UpdateUserDTO updatedUser, string currentPassword);


}