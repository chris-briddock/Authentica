using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="ClientApplication"/>.
/// </summary>
public class ClientApplicationConfiguration : IEntityTypeConfiguration<ClientApplication>
{
    /// <summary>
    /// Configures the entity framework mapping for <see cref="ClientApplication"/>.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<ClientApplication> builder)
    {
        builder.ToTable("SYSTEM_IDENTITY_CLIENT_APPLICATIONS", opt =>
        {
            opt.IsTemporal();
        });

        builder.HasKey(ca => ca.Id);

        builder.Property(ca => ca.Id)
               .HasMaxLength(36);

        builder.Property(ca => ca.ClientId)
             .HasMaxLength(36)
             .HasColumnName("client_id")
             .IsRequired();

        builder.Property(ca => ca.ClientSecret)
               .HasMaxLength(256)
               .HasColumnName("client_secret");

        builder.Property(ca => ca.Name)
               .HasMaxLength(100)
               .HasColumnName("name")
               .IsRequired();

        builder.Property(ca => ca.RedirectUri)
                .HasMaxLength(256)
                .HasColumnName("redirect_uri")
                .IsRequired();
       
        builder.Property(ca => ca.ConcurrencyStamp)
               .HasMaxLength(36)
               .HasColumnName("concurrency_stamp")
               .HasDefaultValueSql("NEWID()")
               .ValueGeneratedOnAddOrUpdate();

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

       builder.Property(u => u.CallbackUri)
              .HasColumnName("callback_uri")
              .HasMaxLength(256);

        // Configure the relationship with UserClientApplication
        builder.HasMany(ca => ca.UserClientApplications)
            .WithOne(ucc => ucc.Application)
            .HasForeignKey(ucc => ucc.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}