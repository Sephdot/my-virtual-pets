namespace my_virtual_pets_api.Entities
{
    public class LocalUser
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public GlobalUser GlobalUser { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }  

        public string Password { get; set; }

    }
}
