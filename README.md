# dotnet-aspire-existing-project-demo

Demo repository for showing how an existing non-Aspire application can be prepared for an eventual Aspire migration.

## ExistingBlazorApp (Non-Aspire Baseline)

`/src/ExistingBlazorApp` is a brand-new example app that intentionally does **not** include Aspire packages or orchestration.

### Features

- Blazor Server-based app (`dotnet new blazor --interactivity Server`)
- Entity Framework Core with SQL Server
- Hybrid Cache for app-level caching
- StackExchangeRedis distributed cache backing
- Azure Blob Storage upload workflow
- Example cached data flow on the `/products` page

### Required configuration

Update `src/ExistingBlazorApp/appsettings.json` (or user secrets/environment variables) with your own values:

- `ConnectionStrings:DefaultConnection` (SQL Server)
- `ConnectionStrings:Redis` (Redis endpoint)
- `BlobStorage:ConnectionString` (Azure Storage)
- `BlobStorage:ContainerName` (optional override)

No real secrets are checked into source control.

### Run locally

```bash
dotnet restore
dotnet build "Aspire Demos.slnx"
dotnet run --project /home/runner/work/dotnet-aspire-existing-project-demo/dotnet-aspire-existing-project-demo/src/ExistingBlazorApp/ExistingBlazorApp.csproj
```

Then use:

- `/products` to test EF + Hybrid Cache + Redis
- `/uploads` to test Azure Blob uploads
