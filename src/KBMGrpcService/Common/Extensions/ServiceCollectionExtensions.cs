using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Application.Services;
using KBMGrpcService.Common.Mapping;
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
            services.AddGrpc();

            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IUserService, UserService>();

            services.AddAutoMapper(typeof(MapperProfile));

            return services;
        }
    }
}
