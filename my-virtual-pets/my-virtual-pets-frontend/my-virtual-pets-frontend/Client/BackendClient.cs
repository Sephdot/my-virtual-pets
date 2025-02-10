using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_frontend.Client;

public class BackendClient<T>
{

        public string Url { get; set; }

        public HttpClient client { get; init; }
        
        public AuthenticationStateProvider AuthenticationStateProvider { get; init; }
        
        public BackendClient(string endpoint, AuthenticationStateProvider provider) {
            Url = "https://localhost:7091/" + endpoint;
            client = new HttpClient(); 
            AuthenticationStateProvider = provider;
        }

        public async Task<string?> GetToken()
        {
            var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
            var token = user.FindFirst(ClaimTypes.Hash)?.Value;
            return token;
        }
        
        public async Task<T> GetRequest()
        {
            var token = await GetToken();   
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            try
            {
                T response = await client.GetFromJsonAsync<T>(Url);
                Console.WriteLine($"Response {(response == null ? "is" : "isn't")} null here");
                return response;
            }
            catch (HttpRequestException ex)
            {
                return default;
            }
        }

        public async Task<HttpStatusCode> PutRequest(T updateValue)
        {
            var token = await GetToken();     
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
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

        public async Task<HttpResponseMessage> PostRequest(T postValue)
        {
            var token = await GetToken();     
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            try
            {
                var response = await client.PostAsJsonAsync<T>(Url, postValue);
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }

        public async Task<HttpStatusCode> DeleteRequest()
        {
            var token = await GetToken();     
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            try
            {
                var response = await client.DeleteAsync(Url);
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ServiceUnavailable;
            }

        }



}