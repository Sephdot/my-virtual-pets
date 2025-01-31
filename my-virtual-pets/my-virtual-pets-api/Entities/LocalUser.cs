using System.Text.Json.Serialization;

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
        public required string FirstName { get; set; }

        [JsonPropertyName("lastname")]
        public required string LastName { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }

    }
}
