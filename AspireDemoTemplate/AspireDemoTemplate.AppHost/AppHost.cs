using Projects;

var builder = DistributedApplication.CreateBuilder(args);

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
    .WaitFor(sql)
    .WaitFor(migrations);

builder.AddProject<Projects.AspireDemoTemplate_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
