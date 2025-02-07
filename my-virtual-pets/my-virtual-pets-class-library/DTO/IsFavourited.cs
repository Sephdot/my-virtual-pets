using System.Text.Json.Serialization;

namespace my_virtual_pets_class_library.DTO;

public class IsFavourited
{
    [JsonPropertyName("isFavourite")]
    public bool IsFavourite { get; set; }
}