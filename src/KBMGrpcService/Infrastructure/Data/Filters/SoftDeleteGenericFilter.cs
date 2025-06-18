using KBMGrpcService.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace KBMGrpcService.Infrastructure.Data.Filters
{
    public static class SoftDeleteGenericFilter
    {
        public static void ApplySoftDeleteFilter(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                var deletedAtProp = clrType.GetProperty("DeletedAt", BindingFlags.Public | BindingFlags.Instance);

                if (deletedAtProp != null && deletedAtProp.PropertyType == typeof(DateTime?))
                {
                    var method = typeof(SoftDeleteGenericFilter)
                        .GetMethod(nameof(SetFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(clrType);
                    method.Invoke(null, new object[] { modelBuilder });
                }
            }
        }

        private static void SetFilter<TEntity>(ModelBuilder builder)
            where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var deletedAt = Expression.PropertyOrField(parameter, "DeletedAt");
            var body = Expression.Equal(deletedAt, Expression.Constant(null, typeof(DateTime?)));
            var lambda = Expression.Lambda(body, parameter);
            builder.Entity<TEntity>().HasQueryFilter(lambda);
        }
    }
}
