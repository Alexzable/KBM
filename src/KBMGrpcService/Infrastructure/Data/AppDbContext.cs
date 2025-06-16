using KBMGrpcService.Domain.Entities;
using KBMGrpcService.Infrastructure.Configurations;
using KBMGrpcService.Infrastructure.Data.Filters;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserOrganization> UserOrganizations { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrganizationConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserOrganizationConfig());


            modelBuilder.ApplySoftDeleteFilter();

            base.OnModelCreating(modelBuilder);
        }
    }
}
