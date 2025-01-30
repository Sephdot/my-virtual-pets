﻿using System.Text.Json.Serialization;

namespace my_virtual_pets_api.Entities
{
    public class GlobalUser
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("gdprpermissions")]
        public bool GDPRPermissions { get; set; }

        [JsonPropertyName("datejoined")]
        public DateTime DateJoined { get; set; }

        public List<Pet> Pets { get; set; }
    }
}
