using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Common.Helpers
{
    public class PaginatedList<TResult>
    {
        public int Page { get; internal set; }
        public int PageSize { get; internal set; }
        public long Total { get; internal set; }
        public IEnumerable<TResult> Items { get; internal set; } = Array.Empty<TResult>();

        internal PaginatedList() { }

        public static async Task<PaginatedList<TResult>> CreateAsync(
              IQueryable<TResult> source,
              int page,
              int pageSize)
        {
            var total = await source.LongCountAsync();
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<TResult>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            };
        }

    }
}
