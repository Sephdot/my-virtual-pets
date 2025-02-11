using my_virtual_pets_frontend.Components;
using BlazorBootstrap;
using Blazored.SessionStorage;
using Blazorise;
using Microsoft.AspNetCore.Components.Authorization;
using my_virtual_pets_frontend;
using my_virtual_pets_frontend.Client;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(o =>
    {
        o.MaximumReceiveMessageSize = 1024 * 1024 * 100;
    }).AddInteractiveWebAssemblyComponents();

builder.Services.AddBlazorBootstrap();

builder.Services.AddBlazoredSessionStorage();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

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

app.UseStatusCodePagesWithReExecute("/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// app.UseBlazorFrameworkFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(my_virtual_pets_frontend.Client._Imports).Assembly);

app.Run();
