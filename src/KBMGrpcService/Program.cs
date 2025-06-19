using KBMGrpcService.Common.Extensions;
using KBMGrpcService.Grpc.Handlers;
using KBMGrpcService.Infrastructure.Data;
using Serilog;


var builder = WebApplication.CreateBuilder(args)
    .UseSerilogLogging();

Log.Information("**Configuration Application:** Starting KBMGrpcService...");

IConfiguration configuration = builder.Configuration;


builder.Services.AddPersistence(configuration);
builder.Services.AddInfrastructureServices(configuration);

var app = builder.Build();

await app.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
app.MapGrpcService<OrganizationController>();
app.MapGrpcService<UserController>();
app.MapGet("/", () =>   "Communication with gRPC endpoints must be made through a gRPC client. " +
                        "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.LogApplicationStarted();

app.Run();

