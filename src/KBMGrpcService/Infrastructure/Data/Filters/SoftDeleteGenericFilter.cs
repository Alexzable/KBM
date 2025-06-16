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
            // Apply filter to all auditable types
            foreach (var et in modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(Auditable).IsAssignableFrom(t.ClrType) ||
                            (t.ClrType.IsGenericType && typeof(Entity<>).MakeGenericType(t.ClrType.GenericTypeArguments).IsAssignableFrom(t.ClrType))))
            {
                var method = typeof(SoftDeleteGenericFilter)
                    .GetMethod(nameof(SetFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(et.ClrType);
                method.Invoke(null, new object[] { modelBuilder });
            }
        }

        // Generic soft-delete filter for auditable entity
        private static void SetFilter<TEntity>(ModelBuilder builder)
            where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var deletedAt = Expression.PropertyOrField(parameter, nameof(Auditable.DeletedAt));
            var body = Expression.Equal(deletedAt, Expression.Constant(null, typeof(DateTime?)));
            var lambda = Expression.Lambda(body, parameter);
            builder.Entity<TEntity>().HasQueryFilter(lambda);
        }
    }
}
