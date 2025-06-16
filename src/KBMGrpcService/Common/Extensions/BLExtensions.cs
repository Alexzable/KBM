using AutoMapper;
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

    }
}
