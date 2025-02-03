using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_virtual_pets_class_library.DTO;

public class UserLoginDTO
{
    [JsonPropertyName("username")]
    [Required]
    public string Username { get; set; }
    
    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; }
}