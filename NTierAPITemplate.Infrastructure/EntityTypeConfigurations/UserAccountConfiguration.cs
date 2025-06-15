using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NTierAPITemplate.Domain.Entities;

namespace NTierAPITemplate.Infrastructure.EntityTypeConfigurations
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            // Map to the standard AspNetUsers table
            builder.ToTable("AspNetUsers");

            // Configure your custom properties
            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.ZipCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(u => u.ReferralCode)
                   .HasMaxLength(50);

            builder.Property(u => u.ProfileImageUrl)
                   .HasMaxLength(500);

            // If you want a unique index on email/user name, Identity already does this:
            // builder.HasIndex(u => u.Email).IsUnique();
            // builder.HasIndex(u => u.UserName).IsUnique();
        }
    }
}
