namespace ImageRecognition
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
    }

}
