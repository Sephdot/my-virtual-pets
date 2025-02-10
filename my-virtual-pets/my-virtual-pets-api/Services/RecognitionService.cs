using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using my_virtual_pets_class_library.Enums;
using my_virtual_pets_api.Services.Interfaces;

namespace ImageRecognition
{
    public class RecognitionService : IRecognitionService
    {
        public string Model { get; set; }
        private string ApiKey { get; set; }
        private IConfiguration Configuration { get; set; }
        public RecognitionService(IConfiguration configuration)
        {
            Model = "animals";
            ApiKey = null;
            Configuration = configuration;
        }
        public RecognitionService()
        {
            Model = "animals";
            ApiKey = null;
        }

        public async Task<IPredicted?> CheckImageInput(string imageLocation)
        {
            var result = await CheckImage(imageLocation, null);
            var deserializedResult = await Deserialize(result);
            if (deserializedResult != null)
            {
                if (CheckIfAnimal(deserializedResult)) return deserializedResult;
            }
            return null;
        }
        public async Task<IPredicted?> CheckImageInput(byte[] imageData)
        {
            //return await CheckImage(null, imageData);
            var result = await CheckImage(null, imageData);
            var deserializedResult = await Deserialize(result);
            if (deserializedResult != null)
            {
                Console.WriteLine("deserialized image isn't null");
                if (CheckIfAnimal(deserializedResult))
                {
                    Console.WriteLine("deserialized image is an animal");
                    return deserializedResult;
                }
            }
            return null;
        }

        private async Task<string?> CheckImage(string? imageLocation, byte[]? imageData)
        {
            try
            {
                ApiKey = Configuration["dragoneyeApiKey"] ?? throw new Exception("DragonEye API Key could not be found.");
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.dragoneye.ai/predict");
                request.Headers.Add("Authorization", $"Bearer {ApiKey}");
                var content = new MultipartFormDataContent();

                if (imageLocation != null)
                {
                    bool imageIsUrl = CheckIfUrl(imageLocation);
                    if (imageIsUrl && imageLocation != null)
                    {
                        content.Add(new StringContent(imageLocation), "image_url");
                    }
                }
                else
                {
                    content.Add(new ByteArrayContent(imageData), "image_file", "image.jpeg");

                }

                content.Add(new StringContent($"dragoneye/animals"), "model_name");
                request.Content = content;
                var response = await client.SendAsync(request);
                // Console.WriteLine(await response.Content.ReadAsStringAsync());
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool CheckIfUrl(string imageLocation)
        {
            var urlRegex = new Regex(
                            @"^(https?|ftps?):\/\/(?:[a-zA-Z0-9]" +
                    @"(?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}" +
                    @"(?::(?:0|[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}" +
                    @"|65[0-4]\d{2}|655[0-2]\d|6553[0-5]))?" +
                    @"(?:\/(?:[-a-zA-Z0-9@%_\+.~#?&=]+\/?)*)?$",
            RegexOptions.IgnoreCase);
            bool result = urlRegex.IsMatch(imageLocation);
            return result;
        }

        public async Task<IPredicted?> Deserialize(string predictionJson)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(predictionJson);
            MemoryStream stream = new MemoryStream(byteArray);
            Root predictionObj = await JsonSerializer.DeserializeAsync<Root>(stream);
            Console.WriteLine(predictionObj.predictions);
            if (predictionObj.predictions.Count > 1)
            {
                throw new ArgumentException("Image contains more than one subject."); // only argumentexception don't change it
            }
            IPredicted jsonData = predictionObj.predictions[0].category;
            Console.WriteLine(jsonData.children.Count);
            while (jsonData.children.Count > 0)
            {
                Console.WriteLine(jsonData.children[0].name);
                jsonData = jsonData.children[0];
            }
            Random random = new Random();
            jsonData.rarity = random.Next(0, 100);
            Console.WriteLine(jsonData.GetType());
            Console.WriteLine(jsonData.name);
            Console.WriteLine(jsonData.rarity);
            return jsonData;
        }

        public bool CheckIfAnimal(IPredicted animal)
        {
            // List<string> validAnimals = new List<string>{  "cat", "dog", "fish", "rabbit", "horse" }
            PetType[] petTypes = Enum.GetValues(typeof(PetType)).Cast<PetType>().ToArray();
            List<string> validAnimals = new List<string>();
            Console.WriteLine("------------Accepted Pet Types------------");
            for (int i = 0; i < petTypes.Length; i++)
            {
                validAnimals.Add(petTypes[i].ToString().ToLower().Replace('_', ' '));
                Console.Write($"[{i}: {validAnimals[i]}], ");
            }
            Console.WriteLine("\n------------------------------------------");
            try
            {
                var result = validAnimals.Contains(animal.name);
                return result;
            }
            catch (NullReferenceException ex)
            {
                return false;
            }
        }
    }

    public class Category : IPredicted
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

    public class Child : IPredicted
    {
        public long id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public object score { get; set; }
        public double uncalibrated_score { get; set; }
        public List<Child> children { get; set; }
        public int rarity {  get; set; }
    }

    public class Prediction
    {
        public Category category { get; set; }
        public List<object> traits { get; set; }
        public List<double> normalizedBbox { get; set; }
    }
    public class Root
    {
        public List<Prediction> predictions { get; set; }
    }

}
