
namespace ImageRecognition
{
    public interface IRecognitionService
    {
        string Model { get; set; }

        Task<string> CheckImageInput(byte[] imageData);
        Task<string> CheckImageInput(string imageLocation);
        Task<IPredicted?> Deserialize(string predictionJson);
    }
}