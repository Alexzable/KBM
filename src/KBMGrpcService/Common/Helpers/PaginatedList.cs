namespace KBMGrpcService.Common.Helpers
{
    public class PaginatedList<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
    }
}
