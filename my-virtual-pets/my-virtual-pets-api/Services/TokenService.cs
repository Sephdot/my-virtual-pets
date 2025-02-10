using Microsoft.Extensions.Caching.Memory;
using my_virtual_pets_api.Entities;
using my_virtual_pets_api.Services.Interfaces;

namespace my_virtual_pets_api.Services;

public class TokenService : ITokenService
{
    private readonly IMemoryCache _cache;
    
    public TokenService(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    public void StoreToken(Guid key, OAuthToken token)
    {
        _cache.Set(key, token, TimeSpan.FromMinutes(3)); 
    }
    
    public OAuthToken GetToken(Guid key)
    {
        return _cache.TryGetValue(key, out OAuthToken token) ? token : null;
    }
}