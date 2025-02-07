namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IRecognitionService
    {
        string Model { get; set; }

        Task<IPredicted> CheckImageInput(byte[] imageData);
        Task<IPredicted> CheckImageInput(string imageLocation);
        Task<IPredicted?> Deserialize(string predictionJson);
    }
}