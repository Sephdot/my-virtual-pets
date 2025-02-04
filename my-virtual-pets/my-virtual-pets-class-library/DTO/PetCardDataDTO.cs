using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using my_virtual_pets_class_library.Enums;

namespace my_virtual_pets_class_library.DTO;

public class PetCardDataDTO
{
    [JsonPropertyName("petid")]
    [Required]
    public required Guid PetId { get; set; }
    
    [JsonPropertyName("petname")]
    [Required]
    public required string PetName { get; set; }

    [JsonPropertyName("imageurl")]
    [Required]
    public required string ImageUrl { get; set; }
    
    [JsonPropertyName("ownerusername")]
    [Required]
    public required string OwnerUsername { get; set; }

    [JsonPropertyName("score")]
    [Required]
    public required int Score { get; set; }
    
    [JsonPropertyName("personality")]
    [Required]
    public required Personality Personality { get; set; }
    
    [JsonPropertyName("pettype")]
    [Required]
    public required PetType PetType { get; set; }
    
    [JsonPropertyName("description")]
    [Required]
    public required string Description { get; set; }
    
    [JsonPropertyName("isfavourited")]
    [Required]
    public required bool IsFavourited { get; set; }
}