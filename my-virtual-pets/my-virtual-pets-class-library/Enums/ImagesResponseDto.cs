using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_virtual_pets_class_library.Enums;

public class ImagesResponseDto
{
    [Required]
    [JsonPropertyName("ImageUrl")]
    public required string ImageUrl { get; set; }
    [Required]
    [JsonPropertyName("PetType")]
    public required PetType PetType { get; set; }
}
