using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace PrototypeFunctionality.Models
{
    public class PrototypeFunctionalityDbContext : IdentityDbContext<ApplicationUser>
    {
        public PrototypeFunctionalityDbContext(DbContextOptions<PrototypeFunctionalityDbContext> options) : base(options)
        {

        }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}