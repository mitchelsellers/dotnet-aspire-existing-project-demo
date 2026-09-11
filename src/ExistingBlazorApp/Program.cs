using Azure.Storage.Blobs;
using ExistingBlazorApp.Components;
using ExistingBlazorApp.Data;
using ExistingBlazorApp.Services;
using ExistingBlazorApp.Storage;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ExistingBlazorAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ExistingBlazorApp";
});

builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(5),
        LocalCacheExpiration = TimeSpan.FromMinutes(1)
    };
});

builder.Services.Configure<BlobStorageOptions>(builder.Configuration.GetSection(BlobStorageOptions.SectionName));

builder.Services.AddSingleton(static serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IConfiguration>()
        .GetSection(BlobStorageOptions.SectionName)
        .Get<BlobStorageOptions>() ?? new BlobStorageOptions();

    return new BlobServiceClient(options.ConnectionString);
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await using (var scope = app.Services.CreateAsyncScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    var context = scope.ServiceProvider.GetRequiredService<ExistingBlazorAppDbContext>();

    try
    {
        await DbInitializer.InitializeAsync(context);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex,
            "Database initialization failed. Update ConnectionStrings:DefaultConnection before using data features.");
    }
}

app.Run();
