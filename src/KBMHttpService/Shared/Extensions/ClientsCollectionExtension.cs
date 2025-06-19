using KBMGrpcService.Grpc;

namespace KBMHttpService.Shared.Extensions
{
    public static class ClientsCollectionExtension
    {
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            var grpcUrl = configuration["Grpc:KBMGrpcServiceUrl"]
                   ?? throw new InvalidOperationException("Missing configuration value for 'Grpc:KBMGrpcServiceUrl'.");

            var uri = new Uri(grpcUrl);

            services.AddGrpcClient<UserService.UserServiceClient>(options =>
            {
                options.Address = uri;
            });
            services.AddGrpcClient<OrganizationService.OrganizationServiceClient>(options =>
            {
                options.Address = uri;
            });

            return services;
        }
    }
}
