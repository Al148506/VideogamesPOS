using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideogamesPOS.Models;

namespace VideogamesPOS.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets para tus entidades
        public DbSet<User> Users { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Videogame> Videogames { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetails> SaleDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sale>()
                .Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleDetails>()
                .Property(sd => sd.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Videogame>()
                .Property(v => v.Price)
                .HasPrecision(18, 2);

            // Si usas relaciones muchos a muchos
            modelBuilder.Entity<Videogame>()
                .HasMany(v => v.Platforms)
                .WithMany(p => p.Videogames)
                .UsingEntity(j => j.ToTable("VideogamePlatforms"));

            modelBuilder.Entity<Videogame>()
                .HasMany(v => v.Genres)
                .WithMany(g => g.Videogames)
                .UsingEntity(j => j.ToTable("VideogameGenres"));
        }



    }
}
