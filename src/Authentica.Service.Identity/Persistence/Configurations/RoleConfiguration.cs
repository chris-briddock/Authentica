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

       builder.Property(u => u.IsDeleted)
              .HasColumnName("is_deleted")
              .IsRequired();
        
        builder.Property(u => u.DeletedOnUtc)
               .HasColumnName("deleted_on_utc");

        builder.Property(u => u.DeletedBy)
               .HasColumnName("deleted_by")
               .HasMaxLength(36);

        builder.Property(u => u.CreatedOnUtc)
               .HasColumnName("created_on_utc")
               .HasDefaultValueSql("GETUTCDATE()")
               .ValueGeneratedOnAdd();

        builder.Property(u => u.CreatedBy)
               .HasColumnName("created_by")
               .HasMaxLength(36);

        builder.Property(u => u.ModifiedOnUtc)
               .HasColumnName("modified_on_utc");

        builder.Property(u => u.ModifiedBy)
               .HasColumnName("modified_by")
               .HasMaxLength(36);

        builder.HasMany(r => r.RoleClaims)
               .WithOne(rc => rc.Role)
               .HasForeignKey(rc => rc.RoleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
