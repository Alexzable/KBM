namespace KBMGrpcService.Data.Seeding
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync(AppDbContext context);
    }
}
