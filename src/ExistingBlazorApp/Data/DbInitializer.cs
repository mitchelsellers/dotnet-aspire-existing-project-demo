using ExistingBlazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExistingBlazorApp.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ExistingBlazorAppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        if (await dbContext.Products.AnyAsync(cancellationToken))
        {
            return;
        }

        dbContext.Products.AddRange(
            new Product { Name = "Laptop Dock", Description = "USB-C docking station", Price = 179.99m, LastUpdatedUtc = DateTime.UtcNow },
            new Product { Name = "4K Monitor", Description = "27-inch IPS panel", Price = 329.00m, LastUpdatedUtc = DateTime.UtcNow },
            new Product { Name = "Wireless Keyboard", Description = "Compact mechanical keyboard", Price = 89.50m, LastUpdatedUtc = DateTime.UtcNow }
        );

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
