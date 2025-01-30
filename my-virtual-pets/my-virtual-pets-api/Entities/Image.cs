using System.Reflection.Metadata;

namespace my_virtual_pets_api.Entities
{
    public class Image
    {
        public Guid Id { get; set; }

        public required byte[] ImageObj { get; set; }

    }
}
