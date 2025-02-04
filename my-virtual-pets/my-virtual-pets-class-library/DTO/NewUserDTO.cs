using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_virtual_pets_class_library.DTO;

public class NewUserDTO
{
    [JsonPropertyName("firstname")]
    [Required]
    public string FirstName { get; set; }

    [JsonPropertyName("lastname")]
    [Required]
    public string LastName { get; set; }
    
    [JsonPropertyName("email")]
    [Required]
    public string Email { get; set; }

    [JsonPropertyName("username")]
    [Required]
    public string Username { get; set; }
    
    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; }

    [JsonPropertyName("gdprpermissions")]
    [Required]
    public bool GDPRPermissions { get; set; }

}
    