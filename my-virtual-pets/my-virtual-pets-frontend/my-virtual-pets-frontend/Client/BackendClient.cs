using System.Net;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_frontend.Client;

public class BackendClient<T>
{

        public string Url { get; set; }

        public HttpClient client { get; init; }
        
        public BackendClient(string endpoint) {
            Url = "https://localhost:7091/" + endpoint;
            client = new HttpClient(); 
        }

        public async Task<T> GetRequest()
        {
            try
            {
                T response = await client.GetFromJsonAsync<T>(Url);
                return response;
            }
            catch (HttpRequestException ex)
            {
                return default;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<HttpStatusCode> PutRequest(T updateValue)
        {
            try
            {
                var response = await client.PutAsJsonAsync<T>(Url, updateValue);
                return response.StatusCode; 
            }
            catch (HttpRequestException ex)
            {
                return HttpStatusCode.ServiceUnavailable;
            }
        }

        public async Task<HttpStatusCode> PostRequest(T postValue)
        {
            try
            {
                var response = await client.PostAsJsonAsync<T>(Url, postValue);
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ServiceUnavailable;
            }
        }

        public async Task<HttpStatusCode> DeleteRequest()
        {
            try
            {
                Console.WriteLine(Url);
                var response = await client.DeleteAsync(Url);
                Console.WriteLine(response.StatusCode);
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ServiceUnavailable;
            }

        }



}