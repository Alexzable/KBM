namespace KBMHttpService.API.Features.Organization.Models.Responses
{
    public class OrganizationsResponse
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public long Total { get; set; }

        public List<OrganizationResponse> Items { get; set; } = new();
    }
}
