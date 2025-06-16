using KBMGrpcService.Infrastructure.Data;
using KBMGrpcService.Infrastructure.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            return services;
        }
    }
}
