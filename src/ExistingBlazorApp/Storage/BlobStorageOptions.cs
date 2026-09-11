namespace ExistingBlazorApp.Storage;

public sealed class BlobStorageOptions
{
    public const string SectionName = "BlobStorage";

    public string ConnectionString { get; init; } = string.Empty;

    public string ContainerName { get; init; } = "uploads";

    public long MaxUploadBytes { get; init; } = 10 * 1024 * 1024;
}
