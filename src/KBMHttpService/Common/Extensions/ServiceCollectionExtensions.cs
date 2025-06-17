using KBMHttpService.Clients.Grpc.Organization;
using KBMHttpService.Clients.Grpc.User;
using KBMHttpService.Common.Helpers;

namespace KBMHttpService.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MapperProfile));

            services.AddScoped<IOrganizationGrpcClient, OrganizationGrpcClient>();
            services.AddScoped<IUserGrpcClient, UserGrpcClient>();

            return services;
        }
    }
}
