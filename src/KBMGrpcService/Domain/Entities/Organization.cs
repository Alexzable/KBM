namespace KBMGrpcService.Domain.Entities
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Address { get; set; }

        public ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    }
}
