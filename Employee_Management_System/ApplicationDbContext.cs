using Employee_Management_System.Model;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace Employee_Management_System
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> users { get; set; }
        public DbSet<Designation> designations { get; set; }

        public DbSet<Department> departments { get; set; }

        public DbSet<Employees> empolyees{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Ashish Vaishya",
                    Email = "Ashish@gmail.com",
                    Password = "Ashish_123",
                    Role = "admin"
                });

        }
    }
}