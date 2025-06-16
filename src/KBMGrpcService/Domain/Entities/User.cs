using KBMGrpcService.Domain.Base;
using KBMGrpcService.Domain.ValueObjects;

namespace KBMGrpcService.Domain.Entities
{
    public class User : Entity<Guid>
    {
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public Email Email { get; set; } = null!;

        public ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    }
}
