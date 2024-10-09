using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DAL.Db
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole, string>
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Rgister> Rgisters { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<ActivityModel> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Add this to ensure Identity entities are configured

            // IdentityUserLogin configuration (this ensures that IdentityUserLogin uses composite keys)
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.LoginProvider, login.ProviderKey, login.UserId });

            // IdentityUserRole configuration (if needed)
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(userRole => new { userRole.UserId, userRole.RoleId });

            // IdentityUserToken configuration (if needed)
            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(token => new { token.UserId, token.LoginProvider, token.Name });

            // Your custom entities
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Stocks)
                .HasForeignKey(s => s.ProductId);
        }

    }
}
