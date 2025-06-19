using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using KBMGrpcService.Common.Helpers;
using System.Reflection;

namespace KBMGrpcService.Common.Extensions
{
    public static class BLExtensions
    {
        internal static IMappingExpression<TSource, TDestination> IgnoreUnmapped<TSource, TDestination>(
                this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceProperties = typeof(TSource)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var destinationProperties = typeof(TDestination)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var destProp in destinationProperties)
            {
                if (!sourceProperties.Contains(destProp.Name))
                {
                    expression.ForMember(destProp.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }

        internal static Timestamp ToGrpcTimestamp(this DateTime dateTime)
            => Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));

        internal static Timestamp? ToNullableGrpcTimestamp(this DateTime? dateTime)
            => dateTime.HasValue ? Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc)) : null;

        internal static PaginatedList<TResult> Map<TSource, TResult>(
                 this PaginatedList<TSource> source,
                 Func<TSource, TResult> converter)
        {
            return new PaginatedList<TResult>
            {
                Page = source.Page,
                PageSize = source.PageSize,
                Total = source.Total,
                Items = source.Items.Select(converter).ToList()
            };
        }


    }
}
