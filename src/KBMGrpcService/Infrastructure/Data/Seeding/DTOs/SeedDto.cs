namespace KBMGrpcService.Infrastructure.Data.Seeding.DTOs
{
    public class SeedDto
    {
        public OrganizationSeedDto[] Organizations { get; set; } = Array.Empty<OrganizationSeedDto>();
        public UserSeedDto[] Users { get; set; } = Array.Empty<UserSeedDto>();
        public MembershipSeedDto[] Memberships { get; set; } = Array.Empty<MembershipSeedDto>();
    }

}
