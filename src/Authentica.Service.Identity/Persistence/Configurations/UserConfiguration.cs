using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="User"/>.
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
       /// <summary>
       /// Configures the entity framework mapping for <see cref="User"/>.
       /// </summary>
       /// <param name="builder">The entity type builder used to configure the entity.</param>
       public void Configure(EntityTypeBuilder<User> builder)
       {
              builder.ToTable("SYSTEM_IDENTITY_USERS", opt =>
              {
                     opt.IsTemporal();
              });

              builder.HasKey(u => u.Id);

              builder.Property(u => u.Id)
                     .HasColumnName("id")
                     .HasMaxLength(36)
                     .IsRequired();

              builder.Property(u => u.UserName)
                     .HasColumnName("username")
                     .HasMaxLength(256)
                     .IsRequired();

              builder.Property(u => u.NormalizedUserName)
                     .HasColumnName("normalized_username")
                     .HasMaxLength(256)
                     .IsRequired();

              builder.Property(u => u.Email)
                     .HasColumnName("email")
                     .HasMaxLength(256)
                     .IsRequired();

              builder.Property(u => u.NormalizedEmail)
                     .HasColumnName("normalized_email")
                     .HasMaxLength(256)
                     .IsRequired();

              builder.Property(u => u.PasswordHash)
                     .HasColumnName("password_hash")
                     .HasMaxLength(512)
                     .IsRequired();

              builder.Property(u => u.SecurityStamp)
                     .HasColumnName("security_stamp")
                     .HasMaxLength(32)
                     .IsRequired();

              builder.Property(u => u.ConcurrencyStamp)
                     .HasColumnName("concurrency_stamp")
                     .HasMaxLength(36)
                     .IsRequired();

              builder.Property(u => u.PhoneNumber)
                     .HasColumnName("phone_number")
                     .HasMaxLength(14);

              builder.Property(u => u.EmailConfirmed)
                     .HasColumnName("email_confirmed");

              builder.Property(u => u.PhoneNumberConfirmed)
                     .HasColumnName("phone_number_confirmed");

              builder.Property(u => u.LockoutEnd)
                     .HasColumnName("lockout_end");

              builder.Property(u => u.LockoutEnabled)
                     .HasColumnName("lockout_enabled");

              builder.Property(u => u.AccessFailedCount)
                     .HasColumnName("access_failed_count");

              builder.Property(u => u.TwoFactorEnabled)
                     .HasColumnName("multi_factor_enabled");

              builder.ComplexProperty(u => u.Address)
                  .Property(a => a.Name).HasColumnName("address_name").HasMaxLength(64);
              builder.ComplexProperty(u => u.Address)
                  .Property(a => a.Number).HasColumnName("address_number").HasMaxLength(10);
              builder.ComplexProperty(u => u.Address)
                  .Property(a => a.Street).HasColumnName("address_street").HasMaxLength(200);
              builder.ComplexProperty(u => u.Address)
                  .Property(a => a.City).HasColumnName("address_city").HasMaxLength(100);
              builder.ComplexProperty(u => u.Address)
                  .Property(a => a.State).HasColumnName("address_state").HasMaxLength(100);
              builder.ComplexProperty(u => u.Address)
                  .Property(a => a.Postcode).HasColumnName("address_postcode").HasMaxLength(10);
              builder.ComplexProperty(u => u.Address)
                  .Property(a => a.Country).HasColumnName("address_country").HasMaxLength(100);

              builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
              builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex").IsUnique();

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

              builder.HasMany(u => u.UserClaims)
                     .WithOne(uc => uc.User)
                     .HasForeignKey(uc => uc.UserId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasMany(u => u.UserRoles)
                     .WithOne(ur => ur.User)
                     .HasForeignKey(ur => ur.UserId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasMany(u => u.UserClientApplications)
                     .WithOne(uca => uca.User)
                     .HasForeignKey(uca => uca.UserId)
                     .OnDelete(DeleteBehavior.Cascade);

    }
}
