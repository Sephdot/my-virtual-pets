using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.SessionStorage; 


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<HttpClient>();

await builder.Build().RunAsync();
