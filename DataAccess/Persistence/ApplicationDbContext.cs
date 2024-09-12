using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Products> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Products>().ToTable("products");

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Created).HasColumnName("created");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.First_Name).HasColumnName("first_name");
                entity.Property(e => e.Last_Name).HasColumnName("last_name");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Updated).HasColumnName("updated");
                entity.Property(e => e.Username).HasColumnName("username");
                entity.Property(e => e.Role).HasColumnName("role");
                entity.Property(e => e.Active).HasColumnName("active");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SKU).HasColumnName("SKU");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Picture).HasColumnName("picture");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.Currency).HasColumnName("currency");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
