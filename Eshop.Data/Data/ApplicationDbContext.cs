using System;
using System.Collections.Generic;
using System.Text;
using Eshop.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().Property(x => x.Price).HasColumnType("decimal(10,1)");
            builder.Entity<Product>().Property(x => x.OldPrice).HasColumnType("decimal(10,1)");

            // určení, které sloupce budou v tabulce CategoryProduct sloužit jako klíče
            builder.Entity<CategoryProduct>().HasKey(cp => new
            {
                cp.CategoryId,
                cp.ProductId
            });

            // nastavení One-To-Many vazby pro obě entity
            builder.Entity<CategoryProduct>()
                   .HasOne(cp => cp.Category)
                   .WithMany(c => c.CategoryProducts)
                   .HasForeignKey(cp => cp.CategoryId);

            builder.Entity<CategoryProduct>()
                   .HasOne(cp => cp.Product)
                   .WithMany(p => p.CategoryProducts)
                   .HasForeignKey(cp => cp.ProductId);

            builder.Entity<Category>().HasData
                (
                new Category() { CategoryId = 1, Title = "Oblečení pro panenky 30cm", Url = "oblecky-pro-panenky-30cm", OrderNo = 1, Hidden = false },
                new Category() { CategoryId = 2, Title = "Oblečení pro panenky 36cm", Url = "oblecky-pro-panenky-36cm", OrderNo = 4, Hidden = false },

                new Category() { CategoryId = 3, ParentCategoryId = 1, Title = "Šaty pro panenku 30cm", Url = "saty-pro-panenku-30cm", OrderNo = 2, Hidden = false },
                new Category() { CategoryId = 4, ParentCategoryId = 1, Title = "Sukně pro panenku 30cm", Url = "sukne-pro-panenku-30cm", OrderNo = 3, Hidden = false },
                new Category() { CategoryId = 5, ParentCategoryId = 2, Title = "Trička pro panenku 36cm", Url = "tricka-pro-panenku-36cm", OrderNo = 5, Hidden = false },
                new Category() { CategoryId = 6, ParentCategoryId = 2, Title = "Kraťasy pro panenku 36cm", Url = "kratasy-pro-panenku-36cm", OrderNo = 6, Hidden = false }
                );
        }
    }
}
