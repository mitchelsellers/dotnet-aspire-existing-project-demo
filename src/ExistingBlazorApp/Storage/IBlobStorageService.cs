using Microsoft.AspNetCore.Components.Forms;

namespace ExistingBlazorApp.Storage;

public interface IBlobStorageService
{
    Task<UploadResult> UploadAsync(IBrowserFile file, CancellationToken cancellationToken = default);
}
