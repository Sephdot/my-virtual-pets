using ImageRecognition;

namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IPredicted
    {
        public long id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public object score { get; set; }
        public double uncalibrated_score { get; set; }
        public List<Child> children { get; set; }
        public int rarity { get; set; }
    }

}
