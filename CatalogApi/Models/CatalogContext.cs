using Microsoft.EntityFrameworkCore;
using CatalogApi.Entities;

namespace CatalogApi.Models
{
    public sealed class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Entities.Rating> Ratings { get; set; }
        public DbSet<Collection> Collections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Film>()
                .Property(e => e.Category)
                .HasConversion<string>();
            
            modelBuilder.Entity<Collection>()
                .HasMany(c => c.Films)
                .WithMany(s => s.Collections)
                .UsingEntity(j => j.ToTable("CollectionsFilms"));
        }
    }
}