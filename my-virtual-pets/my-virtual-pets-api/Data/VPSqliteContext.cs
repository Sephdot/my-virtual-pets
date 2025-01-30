﻿using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Entities;
using System.Text.Json; 

namespace my_virtual_pets_api.Data
{
    public class VPSqliteContext : DbContext 
    {
        public DbSet<GlobalUser> GlobalUsers { get; set; }

        public DbSet<LocalUser> LocalUsers {  get; set; }

        public DbSet<AuthUser> AuthUsers { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Image> Images { get; set; } 

        public VPSqliteContext(DbContextOptions<VPSqliteContext> options) : base(options) {
            Database.EnsureCreated(); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GlobalUser>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<LocalUser>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<AuthUser>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Pet>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Image>().Property(e => e.Id).ValueGeneratedOnAdd();

            // Populate with dummy data later 
            modelBuilder.Entity<GlobalUser>().HasData(JsonSerializer.Deserialize<List<GlobalUser>>(File.ReadAllText("Resources/DummyData/GlobalUsers.json")));

        }



    }
}
