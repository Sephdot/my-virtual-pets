using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Repositories.Interfaces;

public interface IUserRepository
{
    public bool ExistsByUsername(string username);
    public Guid CreateNewLocalUser(NewUserDTO newUserDto, Guid globalUserId);
    public Guid CreateNewAuthUser(NewUserDTO newUserDto, Guid globalUserId);
    public string GetPassword(string username);
    public Guid CreateNewGlobalUser(NewUserDTO newUserDto);
    public bool ExistsByEmail(string email);
    public Guid GetUserIdByUsername(string username);

}