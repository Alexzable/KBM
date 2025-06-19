
using KBMGrpcService.Entities.Base;

namespace KBMGrpcService.Entities
{
    public class Organization : Entity<Guid>
    {
        public string Name { get; set; } = null!;
        public string? Address { get; set; }

        public ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    }
}
