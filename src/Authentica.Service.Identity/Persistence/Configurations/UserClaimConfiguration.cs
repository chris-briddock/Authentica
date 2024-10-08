using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="UserClaim"/>.
/// </summary>
public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    /// <summary>
    /// Configures the entity framework mapping for <see cref="UserClaim"/>.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable("SYSTEM_IDENTITY_USER_CLAIMS", opt => 
        {
              opt.IsTemporal();
        });

        builder.HasKey(uc => uc.Id);

        builder.Property(uc => uc.Id)
               .HasColumnName("id")
               .HasMaxLength(36);

        builder.Property(uc => uc.UserId)
               .HasColumnName("user_id")
               .HasMaxLength(36)
               .IsRequired();
       
       builder.Property(uc => uc.ConcurrencyStamp)
              .HasMaxLength(36)
              .HasColumnName("concurrency_stamp")
              .IsConcurrencyToken();

        builder.Property(uc => uc.ClaimType)
               .HasColumnName("claim_type")
               .HasMaxLength(100);

        builder.Property(uc => uc.ClaimValue)
               .HasColumnName("claim_value")
               .HasMaxLength(100);

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

              builder.ComplexProperty(u => u.EntityDeletionStatus)
                     .Property(x => x.DeletedBy)
                     .HasColumnName("deleted_by")
                     .HasMaxLength(36);

              builder.ComplexProperty(u => u.EntityDeletionStatus)
                     .Property(x => x.DeletedOnUtc)
                     .HasColumnName("deleted_on_utc");

              builder.ComplexProperty(u => u.EntityDeletionStatus)
                     .Property(x => x.IsDeleted)
                     .HasColumnName("is_deleted")
                     .IsRequired();
    }
}