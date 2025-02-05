using my_virtual_pets_api.Services.ApiResponses;
using my_virtual_pets_api.Services.Interfaces;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace my_virtual_pets_api.Services
{
    public class RemoveBackgroundService : IRemoveBackgroundService
    {
        private readonly string bgRemoverApiKey;
        public RemoveBackgroundService(IConfiguration configuration)
        {
            bgRemoverApiKey = configuration["BgRemoverApiKey"] ?? throw new Exception("BgRemoverApiKey is missing!");
        }

        public async Task<byte[]?> RemoveBackgroundAsync(byte[] inputImage)
        {
            Stopwatch sw = Stopwatch.StartNew();

            //Make image content
            var imageContent = new ByteArrayContent(inputImage);
            imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "image",
                FileName = "image.jpg"
            };
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            //Add request content
            var content = new MultipartFormDataContent();
            content.Add(imageContent, "image");
            content.Add(new StringContent("cutout"), "output_type");
            content.Add(new StringContent("fit"), "scale");
            content.Add(new StringContent("true"), "auto_center");
            content.Add(new StringContent("10"), "stroke_size");
            content.Add(new StringContent("000000"), "stroke_color");
            content.Add(new StringContent("PNG"), "format");

            //Make request

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://api.picsart.io/tools/1.0/removebg"))
            {
                Content = content
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("X-Picsart-API-Key", bgRemoverApiKey);

            //Send request
            try
            {
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Request Failed! Status Code: {response.StatusCode}");

                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response Content:");
                    Console.WriteLine(errorResponse);
                    return null;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<BgRemoverApiResponse>(responseBody);

                if (responseData == null) throw new Exception("Could not deserialise BgRemover API response!");

                string imageUrl = responseData.Data.Url;
                var outputImage = await DownloadImageAsync(imageUrl);

                sw.Stop();
                Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
                return outputImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error while contacting the server:");
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private async Task<byte[]?> DownloadImageAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine($"Downloading image from {url}");
                try
                {
                    var imageBytes = await client.GetByteArrayAsync(url);
                    Console.WriteLine("Success!");
                    return imageBytes;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed.");
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }
    }
}
