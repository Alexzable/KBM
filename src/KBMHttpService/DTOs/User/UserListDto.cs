namespace KBMHttpService.DTOs.User
{
    public class UserListDto
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public long Total { get; set; }

        public List<UserDto> Items { get; set; } = new();
    }
}
