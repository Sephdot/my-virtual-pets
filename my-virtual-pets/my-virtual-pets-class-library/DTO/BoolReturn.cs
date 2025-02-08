using System.Text.Json.Serialization;

namespace my_virtual_pets_class_library.DTO;

public class BoolReturn
{
    [JsonPropertyName("isTrue")]
    public bool IsTrue { get; set; }
}