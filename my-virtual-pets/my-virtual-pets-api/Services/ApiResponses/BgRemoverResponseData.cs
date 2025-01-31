using System.Text.Json.Serialization;

namespace my_virtual_pets_api.Services.ApiResponses;
public class BgRemoverResponseData
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
