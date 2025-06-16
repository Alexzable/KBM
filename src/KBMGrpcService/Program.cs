using KBMGrpcService.Common.Extensions;
using KBMGrpcService.Infrastructure.Data;
using KBMGrpcService.Services;


var builder = WebApplication.CreateBuilder(args)
    .UseSerilogLogging();


IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddPersistence(configuration);

var app = builder.Build();

await app.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
