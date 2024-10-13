﻿using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.DAL.Db
{
    public class ApplicationContext : IdentityDbContext<Users> 
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        // Define DbSets for your entities
        public DbSet<Rgister> Rgisters { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<ActivityModel> Activities { get; set; }

        // Ensure primary keys are set
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products) 
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Stocks) 
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Supplier>()
              .HasMany(s => s.Products)
              .WithOne(p => p.Supplier)
              .HasForeignKey(p => p.SupplierId);
        }

    }
}
