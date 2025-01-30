namespace my_virtual_pets_api.Entities
{
    public class GlobalUser
    {

        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool GDPRPermissions { get; set; }

        public DateTime DateJoined { get; set; }

        public List<Pet> Pets { get; set; }
    }
}
