using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configures the properties and relationships of the <see cref="PasskeyCredential"/> entity.
/// </summary>
public class PasskeyCredentialConfiguration : IEntityTypeConfiguration<PasskeyCredential>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PasskeyCredential> builder)
    {
        builder.ToTable("SYSTEM_IDENTITY_USER_PASSKEY_CREDENTIAL", opt => opt.IsTemporal());

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .HasMaxLength(36);

        builder.Property(x => x.CredentialId)
               .HasColumnName("credential_id")
               .IsRequired();

        builder.Property(x => x.PublicKey)
               .HasColumnName("public_key")
               .IsRequired();

        builder.Property(x => x.UserHandle)
               .HasColumnName("user_handle")
               .IsRequired();

        builder.Property(x => x.SignatureCounter)
               .HasColumnName("signature_counter")
               .IsRequired();

        builder.Property(x => x.CredType)
               .HasColumnName("cred_type")
               .HasDefaultValue("public-key")
               .IsRequired();

        builder.Property(x => x.CreatedDate)
               .HasColumnName("created_on_utc")
               .HasDefaultValueSql("GETUTCDATE()")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.AaGuid)
               .HasColumnName("authenticator_id")
               .HasMaxLength(36)
               .IsRequired();
    }
}
