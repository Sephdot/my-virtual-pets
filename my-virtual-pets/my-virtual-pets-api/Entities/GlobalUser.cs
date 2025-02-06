using Microsoft.EntityFrameworkCore;
using my_virtual_pets_class_library.DTO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_virtual_pets_api.Entities
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class GlobalUser
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("username")]
        [Required]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        [Required]
        public string Email { get; set; }

        [JsonPropertyName("gdprpermissions")]
        [Required]
        public bool GDPRPermissions { get; set; }

        [JsonPropertyName("datejoined")]
        [Required]
        public DateTime DateJoined { get; set; }

        public List<Pet> Pets { get; set; }

        public List<Favourite> Favourites { get; set; } = [];

        public GlobalUser()
        {
        }

        public GlobalUser(NewUserDTO newUserDto)
        {
            Username = newUserDto.Username;
            Email = newUserDto.Email;
            GDPRPermissions = newUserDto.GDPRPermissions;
            DateJoined = DateTime.UtcNow;
        }

    }
}
