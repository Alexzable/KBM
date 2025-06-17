using KBMGrpcService.Common.Constants;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Serilog;

namespace KBMGrpcService.Common.Extensions
{
    public static class LoggingExtensions
    {
        public static WebApplicationBuilder UseSerilogLogging(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.File(
                    path: builder.Configuration["Serilog:WriteTo:1:Args:path"] ?? AppConstants.SerilogPath,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: builder.Configuration["Serilog:WriteTo:1:Args:outputTemplate"]
                        ?? "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            builder.Host.UseSerilog();


            return builder;
        }

        public static WebApplication LogApplicationStarted(this WebApplication app)
        {
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                var server = app.Services.GetRequiredService<IServer>();
                var addresses = server.Features
                                      .Get<IServerAddressesFeature>()!
                                      .Addresses;

                app.Logger.LogInformation(
                    "Configuration Application: KBMGrpcService is running on {Urls}",
                    string.Join(", ", addresses));
                app.Logger.LogInformation(new string('-', 50));

            });

            return app;
        }
    }
}
