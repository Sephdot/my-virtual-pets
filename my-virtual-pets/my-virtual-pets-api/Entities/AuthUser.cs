namespace my_virtual_pets_api.Entities
{
    public class AuthUser
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public GlobalUser GlobalUser { get; set; }

        public string Auth0Id { get; set; }

        public string FullName { get; set; }

    }
}
