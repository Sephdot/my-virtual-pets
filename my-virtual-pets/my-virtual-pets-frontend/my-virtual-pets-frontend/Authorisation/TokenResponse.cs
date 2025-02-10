using System.Text.Json.Serialization;

namespace my_virtual_pets_frontend.Client;

public class TokenResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    
}