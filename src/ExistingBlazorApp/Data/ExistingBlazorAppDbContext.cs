using ExistingBlazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExistingBlazorApp.Data;

public sealed class ExistingBlazorAppDbContext(DbContextOptions<ExistingBlazorAppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(x => x.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(1000);

            entity.Property(x => x.Price)
                .HasPrecision(10, 2);
        });
    }
}
