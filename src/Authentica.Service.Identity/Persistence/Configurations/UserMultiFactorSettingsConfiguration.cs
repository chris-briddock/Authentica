using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration for <see cref="UserMultiFactorSettings"/>
/// </summary>
public sealed class UserMultiFactorSettingsConfiguration : IEntityTypeConfiguration<UserMultiFactorSettings>
{
    /// <summary>
    /// Configures the entity framework mapping for <see cref="UserMultiFactorSettings"/>.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<UserMultiFactorSettings> builder)
    {
        // Define the table name and schema
        builder.ToTable("SYSTEM_IDENTITY_USER_MFA_SETTINGS", opt => opt.IsTemporal());

        // Set the primary key
        builder.HasKey(mfa => mfa.Id);

        builder.HasOne(mfa => mfa.User)
               .WithOne(u => u.UserMultiFactorSettings)
               .HasForeignKey<UserMultiFactorSettings>(mfa => mfa.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        // Configure the properties
        builder.Property(mfa => mfa.Id)
               .HasColumnName("id")
               .IsRequired();

        builder.Property(mfa => mfa.UserId)
               .HasColumnName("user_id")
               .IsRequired();

        builder.Property(mfa => mfa.MultiFactorEmailEnabled)
               .HasColumnName("email_enabled")
               .IsRequired();

        builder.Property(mfa => mfa.MultiFactorAuthenticatorEnabled)
               .HasColumnName("authenticator_enabled")
               .IsRequired();

        builder.Property(mfa => mfa.MultiFactorPasskeysEnabled)
               .HasColumnName("passkeys_enabled")
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

        builder.ComplexProperty(u => u.EntityModificationStatus)
               .Property(x => x.ModifiedBy)
               .HasColumnName("modified_by")
               .HasMaxLength(36);

        builder.ComplexProperty(u => u.EntityModificationStatus)
               .Property(x => x.ModifiedOnUtc)
               .HasColumnName("modified_on_utc")
               .HasDefaultValueSql("GETUTCDATE()")
               .ValueGeneratedOnUpdate();
        
        builder.Property(e => e.ConcurrencyStamp)
              .HasColumnName("concurrency_stamp")
              .HasMaxLength(36)
              .IsConcurrencyToken();
        // Add a unique index on UserId.
        builder.HasIndex(mfa => mfa.UserId)
               .IsUnique();
    }
}
