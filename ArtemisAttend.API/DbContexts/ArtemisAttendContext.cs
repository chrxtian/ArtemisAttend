using ArtemisAttend.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ArtemisAttend.API.DbContexts
{
    public class ArtemisAttendContext : DbContext
    {
        public ArtemisAttendContext(DbContextOptions<ArtemisAttendContext> options)
           : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed the database with dummy data
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                    FirstName = "Main",
                    LastName = "Admin",
                    Email = "madmin@test.com",
                    CreatedDate = new DateTime(2020, 12, 01)
                    
                },
                new User()
                {
                    Id = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                    FirstName = "Akemi",
                    LastName = "Uzumaki",
                    Email = "auzumaki@test.com",
                    CreatedDate = new DateTime(2020, 12, 02)                    
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
