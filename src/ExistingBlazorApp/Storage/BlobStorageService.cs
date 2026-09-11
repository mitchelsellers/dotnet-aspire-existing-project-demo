using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;

namespace ExistingBlazorApp.Storage;

public sealed class BlobStorageService(
    BlobServiceClient blobServiceClient,
    IOptions<BlobStorageOptions> options) : IBlobStorageService
{
    public async Task<UploadResult> UploadAsync(IBrowserFile file, CancellationToken cancellationToken = default)
    {
        var blobOptions = options.Value;

        if (string.IsNullOrWhiteSpace(blobOptions.ConnectionString))
        {
            throw new InvalidOperationException("Blob storage connection is not configured.");
        }

        var containerClient = blobServiceClient.GetBlobContainerClient(blobOptions.ContainerName);
        await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        var safeFileName = Path.GetFileName(file.Name);
        var blobName = $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}-{safeFileName}";
        var blobClient = containerClient.GetBlobClient(blobName);

        await using var stream = file.OpenReadStream(blobOptions.MaxUploadBytes, cancellationToken);
        await blobClient.UploadAsync(stream, cancellationToken: cancellationToken);

        return new UploadResult
        {
            OriginalFileName = safeFileName,
            BlobName = blobName,
            BlobUri = blobClient.Uri.ToString()
        };
    }
}
