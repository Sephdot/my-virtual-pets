using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;

namespace my_virtual_pets_frontend.Components;

public class CustomAuthenticationState : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;
 
    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationState(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _sessionStorage.GetItemAsync<string>("jwtToken");
        var user = await _sessionStorage.GetItemAsync<string>("currentUser");
        
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(_currentUser);
        }

        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user) }, "jwt");
        _currentUser = new ClaimsPrincipal(identity);

        return new AuthenticationState(_currentUser);
    }

    public async Task SetTokenAsync(string token)
    {
        await _sessionStorage.SetItemAsync("jwtToken", token);
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

}
