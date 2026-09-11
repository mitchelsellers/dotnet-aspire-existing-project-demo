using ExistingBlazorApp.Models;

namespace ExistingBlazorApp.Services;

public interface IProductService
{
    Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken = default);

    Task<Product> AddProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
}
