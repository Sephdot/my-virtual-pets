namespace my_virtual_pets_api.Entities;

public class OAuthToken
{
    public string Token { get; set; }
    
    public Guid Auth0Id { get; set; }
    
    public string Email { get; set; }
}