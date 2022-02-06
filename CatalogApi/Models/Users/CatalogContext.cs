using Microsoft.EntityFrameworkCore;
using CatalogApi.Entities;

namespace CatalogApi.Models.Users
{
    public class CatalogContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Collection> Collections { get; set; }

        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}