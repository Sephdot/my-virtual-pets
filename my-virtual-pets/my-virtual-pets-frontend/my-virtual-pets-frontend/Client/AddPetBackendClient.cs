using System.Net;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace my_virtual_pets_frontend.Client;

public class AddPetBackendClient<T> : BackendClient<T>
{
        public AddPetBackendClient(string endpoint) 
        : base(endpoint)
        {
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
                return response;
            }
            catch
            {
                Console.WriteLine("Catch block triggered");
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }



}