using my_virtual_pets_api.Data;
using my_virtual_pets_api.Entities;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContext _context;

        public UserRepository(IDbContext context)
        {
            _context = context;
        }

        public bool ExistsByUsername(string username)
        {
            return _context.GlobalUsers.Any(u => u.Username == username);    
        }

        public bool ExistsByEmail(string email)
        {
            return _context.GlobalUsers.Any(u => u.Email == email);
        }

        public Guid CreateNewGlobalUser(NewUserDTO newUserDto)
        {
            GlobalUser newGlobalUser = new GlobalUser(newUserDto);
            _context.GlobalUsers.Add(newGlobalUser);
            _context.SaveChanges();
            return newGlobalUser.Id;
        }
        
        public Guid CreateNewLocalUser(NewUserDTO newUserDto, Guid globalUserId)
        {
            LocalUser newLocalUser = new LocalUser(newUserDto, globalUserId);
            _context.LocalUsers.Add(newLocalUser);
            _context.SaveChanges();
            return newLocalUser.Id;
        }

        public Guid CreateNewAuthUser(NewUserDTO newUserDto, Guid globalUserId)
        {
            throw new NotImplementedException(); 
        }
        
        public string GetPassword(string username)
        {
            var userGuid = _context.GlobalUsers.FirstOrDefault(u => u.Username == username).Id;
            var userPassword = _context.LocalUsers.FirstOrDefault(u => u.GlobalUserId == userGuid).Password;
            return userPassword;
        }

            
    }
}
