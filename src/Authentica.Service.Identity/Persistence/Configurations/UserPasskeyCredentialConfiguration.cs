using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration for <see cref="UserPasskeyCredential"/>.
/// </summary>
public class UserPasskeyCredentialConfiguration : IEntityTypeConfiguration<UserPasskeyCredential>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserPasskeyCredential> builder)
    {
        builder.ToTable("SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL", opt => opt.IsTemporal());

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasColumnName("id")
               .HasMaxLength(36);

        builder.Property(u => u.UserId)
               .HasColumnName("user_id")
               .HasMaxLength(36);

        builder.Property(u => u.PasskeyCredentialId)
               .HasColumnName("passkey_credential_id")
               .HasMaxLength(36)
               .IsRequired();

        builder.HasOne(u => u.User)
               .WithMany(upc => upc.UserPasskeyCredentials)
               .HasForeignKey(upc => upc.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.PasskeyCredential)
               .WithMany(pkc => pkc.UserPasskeyCredential)
               .HasForeignKey(upc => upc.PasskeyCredentialId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
