using Microsoft.AspNetCore.Components.Authorization;
using my_virtual_pets_frontend.Client;

namespace my_virtual_pets_frontend;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private AuthenticationState authenticationState;
    
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
    
    
}

