using KBMGrpcService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Infrastructure.Configurations
{
    public class UserOrganizationConfig : IEntityTypeConfiguration<UserOrganization>
    {
        public void Configure(EntityTypeBuilder<UserOrganization> builder)
        {
            builder.ToTable("UserOrganizations");

            builder.HasKey(uo => new { uo.UserId, uo.OrganizationId });

            builder.HasOne(uo => uo.User)
                   .WithMany(u => u.UserOrganizations)
                   .HasForeignKey(uo => uo.UserId);

            builder.HasOne(uo => uo.Organization)
                   .WithMany(o => o.UserOrganizations)
                   .HasForeignKey(uo => uo.OrganizationId);
        }
    }
}
