using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Entities
{
    public class LocalUser
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("userid")]
        public Guid GlobalUserId { get; set; }

        public GlobalUser GlobalUser { get; set; }

        [JsonPropertyName("firstname")]
        [Required]
        public string FirstName { get; set; }

        [JsonPropertyName("lastname")]
        [Required]
        public string LastName { get; set; }

        [JsonPropertyName("password")]
        [Required]
        public string Password { get; set; }

        public LocalUser() { }

        public LocalUser(NewUserDTO newUserDto, Guid userId)
        {
            GlobalUserId = userId;
            FirstName = newUserDto.FirstName;
            LastName = newUserDto.LastName;
            Password = newUserDto.Password;
        }
        
    }
}
