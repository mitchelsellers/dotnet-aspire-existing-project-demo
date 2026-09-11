namespace ExistingBlazorApp.Models;

public sealed class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;
}
