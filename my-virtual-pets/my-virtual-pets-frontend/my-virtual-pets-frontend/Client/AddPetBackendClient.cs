using System.Net;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace my_virtual_pets_frontend.Client;

public class AddPetBackendClient<T> : BackendClient<T>
{
        public AddPetBackendClient(string endpoint) 
        : base(endpoint)
        {
            cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,  
                // AllowAutoRedirect = true  
            };
        }

        public async Task<HttpResponseMessage> ImageUploadPostRequest(T postValue)
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
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }



}