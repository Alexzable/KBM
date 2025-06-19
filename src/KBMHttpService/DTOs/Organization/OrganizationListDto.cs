namespace KBMHttpService.DTOs.Organization
{
    public class OrganizationListDto
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public long Total { get; set; }

        public List<OrganizationDto> Items { get; set; } = new();
    }
}
