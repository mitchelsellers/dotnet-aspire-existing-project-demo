namespace ExistingBlazorApp.Services;

public sealed class CreateProductRequest
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public decimal Price { get; init; }
}
