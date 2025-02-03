using my_virtual_pets_api.Data;
using my_virtual_pets_api.Repositories.Interfaces;

namespace my_virtual_pets_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContext _context;

        public UserRepository(IDbContext context)
        {
            _context = context;
        }
        
        
    }
}
