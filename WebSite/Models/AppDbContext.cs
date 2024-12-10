using Microsoft.EntityFrameworkCore;
using WebSite.Models;

namespace WebSite.Models
{
    public class AppDbContext : DbContext
    {

        public DbSet<Report> Reports { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }




        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}