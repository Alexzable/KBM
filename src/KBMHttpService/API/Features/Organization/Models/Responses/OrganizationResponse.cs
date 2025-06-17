namespace KBMHttpService.API.Features.Organization.Models.Responses
{
    public class OrganizationResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
