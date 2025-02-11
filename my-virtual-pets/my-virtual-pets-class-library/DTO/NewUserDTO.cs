using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using my_virtual_pets_class_library.DTO.Validators;

namespace my_virtual_pets_class_library.DTO;

public class NewUserDTO
{

    [JsonPropertyName("firstname")]
    [Required(ErrorMessage = "we need your first name!")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastname")]
    [Required(ErrorMessage = "we need your last name!")]
    public string LastName { get; set; }
    
    [JsonPropertyName("email")]
    [Required(ErrorMessage = "we need your email!"), EmailValidator]
    public string Email { get; set; }

    [JsonPropertyName("username")]
    [Required(ErrorMessage = "we need a username!")]
    public string Username { get; set; }
    
    [JsonPropertyName("password")]
    [Required(ErrorMessage = "we need a password!"), PasswordValidator]
    public string Password { get; set; }
    
    [JsonPropertyName("comparepassword")]
    [Required, Compare(nameof(Password), ErrorMessage = "passwords don't match!")]
    public string ComparePassword { get; set; }
    
    [JsonPropertyName("gdprpermissions")]
    [Required]
    public bool GDPRPermissions { get; set; }

}
    