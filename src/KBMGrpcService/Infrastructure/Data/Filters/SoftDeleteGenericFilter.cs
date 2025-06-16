using KBMGrpcService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace KBMGrpcService.Infrastructure.Data.Filters
{
    public static class SoftDeleteGenericFilter
    {
        public static void ApplySoftDeleteFilter(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                         .Where(t => typeof(BaseEntity).IsAssignableFrom(t.ClrType)))
            {
                var genericMethod = typeof(SoftDeleteGenericFilter)
                    .GetMethod(nameof(SetFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);
                genericMethod.Invoke(null, new object[] { modelBuilder });
            }
        }

        private static void SetFilter<TEntity>(ModelBuilder builder)
            where TEntity : BaseEntity
        {
            builder.Entity<TEntity>()
                   .HasQueryFilter(e => EF.Property<DateTime?>(e, nameof(BaseEntity.DeletedAt)) == null);
        }
    }
}
