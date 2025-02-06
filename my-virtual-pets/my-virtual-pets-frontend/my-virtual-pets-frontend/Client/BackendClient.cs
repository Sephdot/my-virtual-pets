using System.Net;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace my_virtual_pets_frontend.Client;

public class BackendClient<T>
{

        public string Url { get; set; }

        public HttpClient client { get; init; }

        public CookieContainer cookieContainer { get; init; }
        
        public BackendClient(string endpoint) {
            cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,  
                // AllowAutoRedirect = true  
            };
            Url = "http://localhost:5138/" + endpoint;
            client = new HttpClient(handler); 
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
                
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Url);
                requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                requestMessage.Content = JsonContent.Create(postValue);
                var response = await client.SendAsync(requestMessage);
                
                // var response = await client.PostAsJsonAsync<T>(Url, postValue);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response);
                var cookies = cookieContainer.GetAllCookies(); 
                foreach (Cookie cookie in cookies)
                {
                    Console.WriteLine($"Cookie Name: {cookie.Name}, Value: {cookie.Value}, Path: {cookie.Path}"); 
                } 
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
                var response = await client.DeleteAsync(Url);
                return response.StatusCode;
            }
            catch
            {
                return HttpStatusCode.ServiceUnavailable;
            }

        }



}