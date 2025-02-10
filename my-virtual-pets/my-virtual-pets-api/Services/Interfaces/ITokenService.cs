using my_virtual_pets_api.Entities;

namespace my_virtual_pets_api.Services.Interfaces;

public interface ITokenService
{
    public void StoreToken(Guid key, OAuthToken token);

    public OAuthToken GetToken(Guid key); 

}
