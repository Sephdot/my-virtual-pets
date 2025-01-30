using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace my_virtual_pets_api.Entities
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class GlobalUser
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("username")]
        public required string Username { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("gdprpermissions")]
        public required bool GDPRPermissions { get; set; }

        [JsonPropertyName("datejoined")]
        public required DateTime DateJoined { get; set; }

        public List<Pet> Pets { get; set; }
    }
}
