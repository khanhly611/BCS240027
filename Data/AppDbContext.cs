using Microsoft.EntityFrameworkCore;
using MID_BCS240027.Models;

namespace MID_BCS240027.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dish_BCS240027> Dishes_BCS240027 { get; set; }

        public DbSet<DishCategory_BCS240027> DishCategories_BCS240027 { get; set; }

        public DbSet<DishImage_BCS240027> DishImages_BCS240027 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dish_BCS240027>()
                .ToTable("Dishes_BCS240027");

            modelBuilder.Entity<DishCategory_BCS240027>()
                .ToTable("DishCategories_BCS240027");

            modelBuilder.Entity<DishImage_BCS240027>()
                .ToTable("DishImages_BCS240027");

            modelBuilder.Entity<Dish_BCS240027>()
                .HasIndex(x => new
                {
                    x.Name,
                    x.DishCategoryId
                })
                .IsUnique();
        }
    }
}