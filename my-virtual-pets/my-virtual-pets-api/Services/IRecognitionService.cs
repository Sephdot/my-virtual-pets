
namespace ImageRecognition
{
    public interface IRecognitionService
    {
        string Model { get; set; }

        Task<IPredicted> CheckImageInput(byte[] imageData);
        Task<IPredicted> CheckImageInput(string imageLocation);
        Task<IPredicted?> Deserialize(string predictionJson);
    }
}