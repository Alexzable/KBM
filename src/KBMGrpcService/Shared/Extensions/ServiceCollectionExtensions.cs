using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Application.Services;
using KBMGrpcService.Common.Mapping;
using KBMGrpcService.Data.Repositories;
using KBMGrpcService.Infrastructure.Data;
using KBMGrpcService.Infrastructure.Data.Seeding;
using KBMGrpcService.Repository;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var useDocker = Environment.GetEnvironmentVariable("USE_DOCKER_DB")?.ToLowerInvariant() == "true";

            var connectionName = useDocker ? "DockerDefaultConnection" : "LocalDefaultConnection";
            var connectionString = configuration.GetConnectionString(connectionName);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();

            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();

            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IUserService, UserService>();

            services.AddAutoMapper(typeof(MapperProfile));

            return services;
        }
    }
}
