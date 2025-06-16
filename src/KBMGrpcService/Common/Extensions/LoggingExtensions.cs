using KBMGrpcService.Common.Constants;
using Serilog;

namespace KBMGrpcService.Common.Extensions
{
    public static class LoggingExtensions
    {
        public static WebApplicationBuilder UseSerilogLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: builder.Configuration["Serilog:WriteTo:1:Args:path"] ?? AppConstants.SerilogPath,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: builder.Configuration["Serilog:WriteTo:1:Args:outputTemplate"]
                        ?? "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            // Leagă Serilog de Host-ul ASP.NET
            builder.Host.UseSerilog();
            return builder;
        }
    }
}
