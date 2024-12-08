using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
/// <summary>
/// Configuration class for the entity framework mapping of <see cref="RoleClaim"/>.
/// </summary>
public sealed class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("SYSTEM_IDENTITY_ROLE_CLAIMS", opt => opt.IsTemporal());

        builder.HasKey(rc => rc.Id);

        builder.Property(rc => rc.Id)
               .HasColumnName("id")
               .HasMaxLength(36);

        builder.Property(rc => rc.RoleId)
               .HasColumnName("role_id")
               .HasMaxLength(36);

        builder.Property(rc => rc.ClaimType)
               .HasColumnName("claim_type")
               .HasMaxLength(100);

        builder.Property(rc => rc.ClaimValue)
               .HasColumnName("claim_value")
               .HasMaxLength(100);

        builder.Property(ca => ca.ConcurrencyStamp)
               .HasMaxLength(36)
               .HasColumnName("concurrency_stamp")
               .IsConcurrencyToken();
    }
}