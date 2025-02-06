using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Entities;
using System.Text.Json;

namespace my_virtual_pets_api.Data
{
    public class VPSqlServerContext : DbContext, IDbContext
    {
        public DbSet<GlobalUser> GlobalUsers { get; set; }

        public DbSet<LocalUser> LocalUsers { get; set; }

        public DbSet<AuthUser> AuthUsers { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Favourite> Favorites { get; set; }


        public VPSqlServerContext(DbContextOptions<VPSqlServerContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseValidationCheckConstraints();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favourite>()
                        .HasKey(f => new { f.GlobalUserId, f.PetId });

            modelBuilder.Entity<Favourite>()
                .HasOne(f => f.GlobalUser)
                .WithMany(g => g.Favourites)
                .HasForeignKey(f => f.GlobalUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Favourite>()
                .HasOne(f => f.Pet)
                .WithMany(p => p.Favourites)
                .HasForeignKey(f => f.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GlobalUser>().HasData(JsonSerializer.Deserialize<List<GlobalUser>>(File.ReadAllText("Resources/DummyData/GlobalUsers.json")));
            modelBuilder.Entity<LocalUser>().HasData(JsonSerializer.Deserialize<List<LocalUser>>(File.ReadAllText("Resources/DummyData/LocalUsers.json")));
            modelBuilder.Entity<Image>().HasData(JsonSerializer.Deserialize<List<Image>>(File.ReadAllText("Resources/DummyData/Images.json")));
            modelBuilder.Entity<Pet>().HasData(JsonSerializer.Deserialize<List<Pet>>(File.ReadAllText("Resources/DummyData/Pets.json")));

        }



    }
}
