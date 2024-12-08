using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="UserRole"/>.
/// </summary>
public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("SYSTEM_LINK_IDENTITY_USER_ROLES", opt => opt.IsTemporal());

        builder.HasKey(ur => new { ur.RoleId, ur.UserId });

        builder.Property(ur => ur.RoleId)
               .HasColumnName("role_id")
               .HasMaxLength(36);

        builder.Property(ur => ur.UserId)
               .HasColumnName("user_id")
               .HasMaxLength(36);

        builder.HasOne(ur => ur.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(ur => ur.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.User)
               .WithMany(u => u.UserRoles)
               .HasForeignKey(ur => ur.UserId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
