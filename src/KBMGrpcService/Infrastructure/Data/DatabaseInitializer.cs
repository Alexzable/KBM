using KBMGrpcService.Infrastructure.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KBMGrpcService.Infrastructure.Data
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

                var seeder = services.GetService<IDatabaseSeeder>();
                if (seeder != null)
                {
                    await seeder.SeedAsync(context);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while initializing the database");
                throw;
            }
        }
    }
}
