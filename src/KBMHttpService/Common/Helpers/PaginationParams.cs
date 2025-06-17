using System.ComponentModel.DataAnnotations;

namespace KBMHttpService.Common.Helpers
{
    public class PaginationParams
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        public string OrderBy { get; set; } = "Id";

        public bool Descending { get; set; } = false;

        public string? QueryString { get; set; }
    }
}
