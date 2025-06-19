using System.ComponentModel.DataAnnotations;

namespace KBMHttpService.Common
{
    public class PaginationParams
    {
        [Range(1, 200)]
        public int Page { get; set; } = 1;

        [Range(1, 25)]
        public int PageSize { get; set; } = 10;

        public string OrderBy { get; set; } = "Id";

        public bool Descending { get; set; } = false;

        public string QueryString
        {
            get => _queryString;
            set => _queryString = value ?? "";
        }
        private string _queryString = "";
    }
}
