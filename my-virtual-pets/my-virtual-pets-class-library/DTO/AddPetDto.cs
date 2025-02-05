using my_virtual_pets_class_library.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_virtual_pets_class_library.DTO;

public class AddPetDTO
{
    [JsonPropertyName("petname")]
    [Required]
    public required string PetName { get; set; }

    [JsonPropertyName("imageurl")]
    [Required]
    public required string ImageUrl { get; set; }

    [JsonPropertyName("ownerid")]
    [Required]
    public required Guid OwnerId { get; set; }

    [JsonPropertyName("personality")]
    [Required]
    public required Personality Personality { get; set; }

    [JsonPropertyName("pettype")]
    [Required]
    public required PetType PetType { get; set; }

    [JsonPropertyName("description")]
    [Required]
    public required string Description { get; set; }
}
