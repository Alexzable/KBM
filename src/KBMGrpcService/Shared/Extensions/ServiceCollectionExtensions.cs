using KBMGrpcService.Data;
using KBMGrpcService.Data.Repositories;
using KBMGrpcService.Data.Seeding;
using KBMGrpcService.Repository;
using KBMGrpcService.Services.Interfaces;
using KBMGrpcService.Shared.Interceptors;
using KBMGrpcService.Shared.Mapping;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Shared.Extensions
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
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<LoggingInterceptor>();
            });

            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IOrganizationService, Services.OrganizationService>();
            services.AddScoped<IUserService, Services.UserService>();

            services.AddAutoMapper(typeof(MapperProfile));

            return services;
        }
    }
}
