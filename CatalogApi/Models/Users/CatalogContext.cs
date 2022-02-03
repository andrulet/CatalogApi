using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CatalogApi.Entities;

namespace CatalogApi.Models
{
    public partial class CatalogContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}