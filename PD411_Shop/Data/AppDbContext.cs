
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Models;

namespace PD411_Shop.Data
{
    //public class AppDbContext : DbContext
    //public class AppDbContext : IdentityDbContext //to work with Identity we need to include it to the DI
    public class AppDbContext : IdentityDbContext<UserModel> //add our class for the Identity with new fields
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        { 
        }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Category
            builder.Entity<CategoryModel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .HasDefaultValue("bi-activity");
            });

            // Product
            builder.Entity<ProductModel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

                entity.Property(e => e.Image)
                .HasMaxLength(255);

                entity.Property(e => e.Color)
                .HasMaxLength(100);

                entity.Property(e => e.Description)
                .HasColumnType("ntext");
            });

            // Relationships
            // Category one to many Products
            builder.Entity<ProductModel>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}