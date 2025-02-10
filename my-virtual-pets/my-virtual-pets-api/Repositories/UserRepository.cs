using Microsoft.EntityFrameworkCore;
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

        public async Task<Guid> GetUserIdByUsername(string username)
        {
            var userId =await _context.GlobalUsers.FirstOrDefaultAsync(u => u.Username == username);
            if (userId == null)
            {
                return Guid.Empty; 
            }
            return userId.Id;
        }

        public async Task<bool> ExistsByUsername(string username)
        {
            return await _context.GlobalUsers.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _context.GlobalUsers.AnyAsync(u => u.Email == email);
        }

        public async Task<Guid> CreateNewGlobalUser(NewUserDTO newUserDto)
        {
            GlobalUser newGlobalUser = new GlobalUser(newUserDto);
            _context.GlobalUsers.Add(newGlobalUser);
           await _context.SaveChangesAsync();
            return newGlobalUser.Id;
        }

        public async Task<Guid> CreateNewLocalUser(NewUserDTO newUserDto, Guid globalUserId)
        {
            LocalUser newLocalUser = new LocalUser(newUserDto, globalUserId);
            _context.LocalUsers.Add(newLocalUser);
           await _context.SaveChangesAsync();
            return newLocalUser.Id;
        }

        public async Task<Guid> CreateNewAuthUser(NewUserDTO newUserDto, Guid globalUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetPassword(string username)
        {
            var userGuid = await _context.GlobalUsers.FirstOrDefaultAsync(u => u.Username == username);
            var userPassword = await _context.LocalUsers.FirstOrDefaultAsync(u => u.GlobalUserId == userGuid.Id);
            return userPassword.Password;
        }

        public async Task<UserDisplayDTO> GetUserDetailsByUserId(Guid userId)
        {
            var user = await _context.GlobalUsers
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var localUser = await _context.LocalUsers
                .Where(lu => lu.GlobalUserId == userId)
                .FirstOrDefaultAsync();

            if (user == null || localUser == null)
            {
                return null;
            }

            var petCount = await _context.Pets
                .Where(p => p.GlobalUserId == userId)
                .CountAsync();

            var userDisplayDTO = new UserDisplayDTO
            {
                Username = user.Username,
                FirstName = localUser.FirstName,
                LastName = localUser.LastName,
                Email = user.Email,
                PetCount = petCount
            };

            return userDisplayDTO;
        }

        public async Task<bool> AddToFavourites(Guid GlobalUserId, Guid PetId)
        {
            //TO DO: better error handling
            var user = await _context.GlobalUsers.SingleOrDefaultAsync(g => g.Id == GlobalUserId);
            if (user == null) throw new KeyNotFoundException("GlobalUser does not exist");

            var pet = await _context.Pets.SingleOrDefaultAsync(p => p.Id == PetId);
            if (pet == null) throw new KeyNotFoundException("Pet not exist");

            var existingFavourite = await _context.Favorites.SingleOrDefaultAsync(f => f.GlobalUserId == GlobalUserId && f.PetId == PetId);
            if (existingFavourite != null) return false;

            var favourite = new Favourite { GlobalUserId = GlobalUserId, PetId = PetId };
            _context.Favorites.Add(favourite);
           await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsFavourited(Guid GlobalUserId, Guid PetId)
        {
            var user = await _context.GlobalUsers.Include(g => g.Favourites).SingleOrDefaultAsync(g => g.Id == GlobalUserId);
            if (user == null) throw new KeyNotFoundException("GlobalUser does not exist");
            return user.Favourites.Any(f => f.PetId == PetId);
        }

        
        
        public async Task<List<Guid>> GetFavoritePetIds(Guid GlobalUserId)
        {
            var user = await _context.GlobalUsers.Include(g => g.Favourites).SingleOrDefaultAsync(g => g.Id == GlobalUserId);
            if (user == null) throw new KeyNotFoundException("GlobalUser does not exist");
            var favouritePetIds = user.Favourites.Select(f => f.PetId).ToList();
            return favouritePetIds;
        }

        public async Task<List<PetCardDataDTO>> GetFavoritePetCards(Guid GlobalUserId)
        {
            var user = await _context.GlobalUsers.Include(g => g.Favourites)
                                            .ThenInclude(f => f.Pet)
                                            .ThenInclude(p => p.Image)
                                            .SingleOrDefaultAsync(g => g.Id == GlobalUserId);
            if (user == null) throw new KeyNotFoundException("GlobalUser does not exist");
            var favouritePets = user.Favourites.Select(f => f.Pet).ToList();
            if (favouritePets.Count == 0) return [];
            return favouritePets.Select(p => Pet.CreatePetCardDto(p)).ToList();

        }

        public async Task<bool> RemoveFromFavourites(Guid GlobalUserId, Guid PetId)
        {
            var user = _context.GlobalUsers.Include(g => g.Favourites);
            if (user == null) throw new KeyNotFoundException("GlobalUser does not exist");

            var pet = await _context.Pets.SingleOrDefaultAsync(p => p.Id == PetId);
            if (pet == null) throw new KeyNotFoundException("Pet does not exist");

            var existingFavourite = await _context.Favorites.SingleOrDefaultAsync(f => f.GlobalUserId == GlobalUserId && f.PetId == PetId);
            if (existingFavourite == null) return false;

            _context.Favorites.Remove(existingFavourite);
           await _context.SaveChangesAsync();
            return true;
        }

        
            public async Task<bool> UpdateUser(UpdateUserDTO updatedUser, string currentPassword)
            {
                var globalUser = await _context.GlobalUsers.FirstOrDefaultAsync(u => u.Id == updatedUser.UserId);
                if (globalUser == null)
                {
                    throw new KeyNotFoundException(" user not found.");
                }

                var localUser = await _context.LocalUsers.FirstOrDefaultAsync(lu => lu.GlobalUserId == updatedUser.UserId);

                if (localUser == null || !BCrypt.Net.BCrypt.Verify(currentPassword, localUser.Password))
                {
                    throw new UnauthorizedAccessException("Current password is incorrect.");
                }


                if (!string.IsNullOrWhiteSpace(updatedUser.NewUsername))
                {
                    if (_context.GlobalUsers.Any(u => u.Username == updatedUser.NewUsername && u.Id != updatedUser.UserId))
                    {
                        throw new InvalidOperationException("Username is already in use.");
                    }
                    globalUser.Username = updatedUser.NewUsername;
                }

                if (!string.IsNullOrWhiteSpace(updatedUser.NewPassword))
                {
                    if (localUser == null)
                    {
                        throw new KeyNotFoundException("user record not found.");
                    }
                    localUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(updatedUser.NewPassword);
                }

                await _context.SaveChangesAsync();
                return true;
            }

    }
}
