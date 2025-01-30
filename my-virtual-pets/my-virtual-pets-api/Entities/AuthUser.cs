using Microsoft.EntityFrameworkCore;

namespace my_virtual_pets_api.Entities
{
    [Index(nameof(Auth0Id), IsUnique = true)]
    public class AuthUser
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public GlobalUser GlobalUser { get; set; }

        public required string Auth0Id { get; set; }

        public required string FullName { get; set; }

    }
}
