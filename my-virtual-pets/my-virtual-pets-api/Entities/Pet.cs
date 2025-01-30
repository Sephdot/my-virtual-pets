using my_virtual_pets_class_library.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_virtual_pets_api.Entities
{
    public class Pet
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public GlobalUser GlobalUser { get; set; }

        public string? Name { get; set; }

        public int? Personality { get; set; }

        public string? Description { get; set; }

        [Column(TypeName ="int")]
        public PetType Type { get; set; }
        
        public Guid ImageId { get; set; }

        public Image Image { get; set; }

    }
}
