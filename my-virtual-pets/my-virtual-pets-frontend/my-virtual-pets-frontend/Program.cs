using my_virtual_pets_frontend.Components;
using BlazorBootstrap;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using my_virtual_pets_frontend;
using my_virtual_pets_frontend.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddBlazorBootstrap();

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<HttpClient>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<CustomAuthenticationService>();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthorizationCore();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// app.UseBlazorFrameworkFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(my_virtual_pets_frontend.Client._Imports).Assembly);

app.Run();
