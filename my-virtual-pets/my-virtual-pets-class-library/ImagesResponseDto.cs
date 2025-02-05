using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_virtual_pets_class_library;

public class ImagesResponseDto
{
    [Required]
    [JsonPropertyName("ImageUrl")]
    public required string imageUrl { get; set; }
    [Required]
    [JsonPropertyName("AnimalType")]
    public required string animalType { get; set; }
}
