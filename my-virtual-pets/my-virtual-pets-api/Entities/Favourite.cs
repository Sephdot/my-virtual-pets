namespace my_virtual_pets_api.Entities
{
    public class Favourite
    {
        public Guid GlobalUserId { get; set; }
        public GlobalUser GlobalUser { get; set; }

        public Guid PetId { get; set; }
        public Pet Pet { get; set; }
    }
}
