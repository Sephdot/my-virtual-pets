namespace my_virtual_pets_api.Entities
{
    public class LocalUser
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public GlobalUser GlobalUser { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }  

        public required string Password { get; set; }

    }
}
