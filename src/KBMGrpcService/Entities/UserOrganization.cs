
using KBMGrpcService.Entities.Base;

namespace KBMGrpcService.Entities
{
    public class UserOrganization : Auditable
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
    }
}
