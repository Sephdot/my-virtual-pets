using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Entities;

namespace my_virtual_pets_api.Data
{
    public interface IDbContext : IDisposable
    {
        public DbSet<GlobalUser> GlobalUsers { get; set; }

        public DbSet<LocalUser> LocalUsers { get; set; }

        public DbSet<AuthUser> AuthUsers { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Favourite> Favorites { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
