using my_virtual_pets_class_library.DTO;
using my_virtual_pets_class_library.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace my_virtual_pets_api.Entities
{
    public class Pet
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("userid")]
        public Guid GlobalUserId { get; set; }

        public GlobalUser GlobalUser { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("personality")]
        [Column(TypeName = "int")]
        public Personality Personality { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("type")]
        [Column(TypeName = "int")]
        public PetType Type { get; set; }


        [JsonPropertyName("imageid")]
        public Guid ImageId { get; set; }

        public Image Image { get; set; }

        //TestComment
        public static PetCardDataDTO CreatePetCardDto(Pet pet)
        {
            PetCardDataDTO petCardDto = new PetCardDataDTO
            {
                PetId = pet.Id,
                PetName = pet.Name,
                ImageUrl = pet.Image.ImageUrl,
                OwnerUsername = pet.GlobalUser.Username,
                Score = 0,
                Personality = pet.Personality,
                PetType = pet.Type,
                Description = pet.Description,
                IsFavourited = false
            };
            return petCardDto;
        }
    }
}
