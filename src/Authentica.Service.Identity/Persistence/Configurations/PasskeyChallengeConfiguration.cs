using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configures the properties and relationships of the <see cref="PasskeyChallenge"/> entity.
/// </summary>
public class PasskeyChallengeConfiguration : IEntityTypeConfiguration<PasskeyChallenge>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PasskeyChallenge> builder)
    {
        // Set the table name and enable temporal table support
        builder.ToTable("SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE", opt => opt.IsTemporal());

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .HasMaxLength(36);

        builder.Property(x => x.ChallengeId)
               .HasColumnName("challenge_id")
               .IsRequired()
               .HasMaxLength(128);

        builder.Property(x => x.Challenge)
               .HasColumnName("challenge")
               .IsRequired();

        builder.Property(x => x.Status)
               .HasColumnName("status")
               .HasMaxLength(16)
               .IsRequired();

        builder.Property(x => x.ExpiresAt)
               .HasColumnName("expires_at")
               .HasDefaultValue(DateTime.UtcNow.AddMinutes(5))
               .IsRequired();

        builder.ComplexProperty(u => u.EntityCreationStatus)
               .Property(x => x.CreatedBy)
               .HasColumnName("created_by")
               .HasMaxLength(36);

        builder.ComplexProperty(u => u.EntityCreationStatus)
               .Property(x => x.CreatedOnUtc)
               .HasColumnName("created_on_utc")
               .HasDefaultValueSql("GETUTCDATE()")
               .ValueGeneratedOnAdd();

    }
}
