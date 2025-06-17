namespace KBMHttpService.API.Features.User.Models.Responses
{
    public class UsersResponse
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public long Total { get; set; }

        public List<UserResponse> Items { get; set; } = new();
    }
}
