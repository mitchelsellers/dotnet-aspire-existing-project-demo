namespace ExistingBlazorApp.Storage;

public sealed class UploadResult
{
    public string OriginalFileName { get; init; } = string.Empty;

    public string BlobName { get; init; } = string.Empty;

    public string BlobUri { get; init; } = string.Empty;
}
