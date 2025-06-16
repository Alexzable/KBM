using KBMGrpcService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Infrastructure.Configurations
{
    public class OrganizationConfig : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Organizations");

            builder.Property(o => o.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(o => o.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(o => o.Address)
                   .HasMaxLength(500);

            builder.HasIndex(o => o.Name)
                   .IsUnique()
                   .HasFilter("[DeletedAt] IS NULL");
        }
    }
}
