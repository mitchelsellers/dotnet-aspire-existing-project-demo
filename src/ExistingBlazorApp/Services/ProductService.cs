using ExistingBlazorApp.Data;
using ExistingBlazorApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace ExistingBlazorApp.Services;

public sealed class ProductService(
    ExistingBlazorAppDbContext dbContext,
    HybridCache cache,
    ILogger<ProductService> logger) : IProductService
{
    private const string ProductListCacheKey = "products:all";

    public async Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting products via hybrid cache key {CacheKey}", ProductListCacheKey);

        return await cache.GetOrCreateAsync(
            ProductListCacheKey,
            async cancel =>
            {
                logger.LogInformation("Cache miss for {CacheKey}, querying SQL database", ProductListCacheKey);

                return (IReadOnlyList<Product>)await dbContext.Products
                    .AsNoTracking()
                    .OrderBy(x => x.Name)
                    .ToListAsync(cancel);
            },
            cancellationToken: cancellationToken);
    }

    public async Task<Product> AddProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Price = request.Price,
            LastUpdatedUtc = DateTime.UtcNow
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(ProductListCacheKey, cancellationToken);

        return product;
    }
}
