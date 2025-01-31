using System.Text.Json.Serialization;

namespace my_virtual_pets_api.Services.ApiResponses;

public class BgRemoverApiResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("data")]
    public BgRemoverResponseData Data { get; set; }
}
