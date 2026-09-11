using Projects;

var builder = DistributedApplication.CreateBuilder(args);

//Setup Emulated Blob Storage
var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

var blobsService = storage.AddBlobs("blobs");
var imagesContainer = storage.AddBlobContainer("images", "images");

//Easy additional parameter values!
var apiKey = builder.AddParameter("my-api-key", secret: true);

//Add SQL
var sql = builder.AddSqlServer("sql")
    //.WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("DefaultConnection", "sqldata");

//Ensure DB Ready to go
var migrations = builder.AddProject<Projects.ApiServiceSetupWorker>("migrations")
    .WithReference(sql)
    .WaitFor(sql);

//Start API only when we have stuff
var apiService = builder.AddProject<Projects.AspireDemoTemplate_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(sql)
    .WithReference(migrations)
    .WithReference(blobsService)
    .WithReference(imagesContainer)
    .WaitFor(sql)
    .WaitFor(migrations)
    .WaitFor(imagesContainer);

builder.AddProject<Projects.AspireDemoTemplate_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
