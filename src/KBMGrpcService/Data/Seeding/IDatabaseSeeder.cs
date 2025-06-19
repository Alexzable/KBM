namespace KBMGrpcService.Infrastructure.Data.Seeding
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync(AppDbContext context);
    }
}
