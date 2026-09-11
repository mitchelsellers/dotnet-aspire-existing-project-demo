var builder = DistributedApplication.CreateBuilder(args);

//Register SQL
var sql = builder.AddSqlServer("sql")
    .AddDatabase("DefaultConnection", "myDb");

var redis = builder.AddRedis("redis")
    .WithRedisInsight();


builder.AddProject<Projects.ExistingBlazorApp>("existingblazorapp")
    .WithReference(sql)
    .WithReference(redis)
    .WaitFor(sql)
    .WaitFor(redis);

builder.Build().Run();
