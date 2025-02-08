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

    
    // public void AuthenticateUser(string username, string token, string userid)
    // {
    //     var identity = new ClaimsIdentity(
    //         [
    //             new Claim(ClaimTypes.Name, username),
    //             new Claim(ClaimTypes.Hash, token),
    //             new Claim(ClaimTypes.NameIdentifier, userid)
    //         ],
    //         "Custom Authentication");
    //     
    //     authenticationState.CurrentUser = new ClaimsPrincipal(identity);
    //
    //     NotifyAuthenticationStateChanged(
    //         Task.FromResult(new AuthenticationState(user)));
    // }
    
}

