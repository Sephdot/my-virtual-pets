using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
using my_virtual_pets_frontend.Client;

namespace my_virtual_pets_frontend;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private AuthenticationState authenticationState;

    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(CustomAuthenticationService customAuthenticationService)
    {
        authenticationState = new AuthenticationState(customAuthenticationService.CurrentUser);
        customAuthenticationService.UserChanged += (newUser) =>
        {
            authenticationState = new AuthenticationState(newUser);
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        };
    }
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(authenticationState);
    
    // public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    // {
    //     
    //     var token = _sessionState.GetToken();
    //     var username = _sessionState.GetUserName() == null ? "NOTCALLED" : _sessionState.GetUserName();
    //             
    //     if (string.IsNullOrEmpty(username))
    //     {
    //         return new AuthenticationState(_currentUser);
    //     }
    //
    //     var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "jwt");
    //     _currentUser = new ClaimsPrincipal(identity);
    //
    //     return new AuthenticationState(_currentUser);
    // }
    
    public void AuthenticateUser(string userIdentifier)
    {
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, userIdentifier),
        ], "Custom Authentication");

        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user)));
    }
    
    //
    // public async Task SetTokenAsync(string token, string userId, string userName)
    // {
    //     
    //     _sessionState.SetJwtToken(token);
    //     _sessionState.SetUserId(userId);
    //     _sessionState.SetUserName(userName);
    //     _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
    //     NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    // }

}

