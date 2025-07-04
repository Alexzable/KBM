﻿using KBMGrpcService.Entities;
using KBMGrpcService.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KBMGrpcService.Data.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(o => o.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .HasConversion(
                       email => email.Value,
                       value => new Email(value))
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(u => u.Username)
                   .IsUnique()
                   .HasFilter("[DeletedAt] IS NULL");
            builder.HasIndex("Email")
                   .IsUnique()
                   .HasFilter("[DeletedAt] IS NULL");
        }
    }
}
