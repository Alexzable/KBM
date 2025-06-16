using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Common.Helpers
{
    public class PaginatedList<TResult>
    {
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public long Total { get; private set; }
        public IEnumerable<TResult> Items { get; private set; } = Array.Empty<TResult>();

        private PaginatedList() { }

        public static async Task<PaginatedList<TResult>> CreateAsync<TSource>(
            IQueryable<TSource> source,
            int page,
            int pageSize,
            IMapper mapper)
        {
            var total = await source.LongCountAsync();
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
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
