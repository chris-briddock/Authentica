using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="Role"/>.
/// </summary>
public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
       /// <inheritdoc/>
       public void Configure(EntityTypeBuilder<Role> builder)
       {
              builder.ToTable("SYSTEM_IDENTITY_ROLES", opt =>
              {
                     opt.IsTemporal();
              });

              builder.HasKey(r => r.Id);

              builder.Property(r => r.Id)
                     .HasColumnName("id")
                     .HasMaxLength(36);

              builder.HasIndex(r => r.NormalizedName)
                     .HasDatabaseName("RoleNameIndex").IsUnique();

              builder.Property(r => r.ConcurrencyStamp)
                     .HasColumnName("concurrency_stamp")
                     .HasMaxLength(36)
                     .IsConcurrencyToken();

              builder.Property(u => u.Name)
                     .HasColumnName("name")
                     .HasMaxLength(100);

              builder.Property(u => u.NormalizedName)
                     .HasColumnName("normalized_name")
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
                     .IsRequired();

              builder.HasMany(r => r.RoleClaims)
                     .WithOne(rc => rc.Role)
                     .HasForeignKey(rc => rc.RoleId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}
