using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using my_virtual_pets_class_library.DTO.Validators;

namespace my_virtual_pets_class_library.DTO;

public class NewUserDTO
{

    [JsonPropertyName("firstname")]
    [Required(ErrorMessage = "We need your first name!")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastname")]
    [Required(ErrorMessage = "We need your last name!")]
    public string LastName { get; set; }
    
    [JsonPropertyName("email")]
    [Required(ErrorMessage = "We need your email!"), EmailValidator]
    public string Email { get; set; }

    [JsonPropertyName("username")]
    [Required(ErrorMessage = "We need a username!")]
    public string Username { get; set; }
    
    [JsonPropertyName("password")]
    [Required(ErrorMessage = "We need a password!"), PasswordValidator]
    public string Password { get; set; }
    
    [JsonPropertyName("comparepassword")]
    [Required, Compare(nameof(Password), ErrorMessage = "Password do not match.")]
    public string ComparePassword { get; set; }
    
    [JsonPropertyName("gdprpermissions")]
    [Required]
    public bool GDPRPermissions { get; set; }

}
    