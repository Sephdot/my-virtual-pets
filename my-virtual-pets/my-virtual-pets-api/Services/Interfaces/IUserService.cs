using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Services.Interfaces;

public interface IUserService
{
    public bool ExistsByUsername(string username);
    public void CreateNewLocalUser(NewUserDTO newUserDto);
    public bool DoesPasswordMatch(UserLoginDTO userLoginDto);
    public bool ExistsByEmail(string email);

}