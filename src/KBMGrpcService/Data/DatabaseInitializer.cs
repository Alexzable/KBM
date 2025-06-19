using KBMGrpcService.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KBMGrpcService.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppDbContext>();

                await context.Database.MigrateAsync();

                Log.Information("**Configuration Application:** Database was successfully migrated");

                var seeder = services.GetService<IDatabaseSeeder>();
                if (seeder != null)
                {
                    await seeder.SeedAsync(context);

                    Log.Information("**Configuration Application:** Trying to synchroniz initial data from JSON files");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "**Configuration Application:** Error initializing database}");
                throw;
            }
        }
    }
}
