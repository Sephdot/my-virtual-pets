using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace my_virtual_pets_api.Entities
{
    public class Image
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("imageobj")]
        [MaxLength(8000)]
        [Column(TypeName = "Binary")]
        public byte[] ImageObj { get; set; }

    }
}
