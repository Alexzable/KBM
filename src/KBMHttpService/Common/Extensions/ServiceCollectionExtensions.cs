using KBMHttpService.Services;
using KBMHttpService.Services.Interfaces;

namespace KBMHttpService.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MapperProfile));

            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
