using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configures the properties and relationships of the <see cref="Session"/> entity.
/// </summary>
public sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    /// <summary>
    /// Configures the entity of type <see cref="Session"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        // Set the table name and enable temporal table support
        builder.ToTable("SYSTEM_IDENTITY_SESSIONS", opt =>
        {
            opt.IsTemporal();
        });

        // Configure the Id property
        builder.Property(e => e.Id)
               .HasColumnName("id")
               .HasMaxLength(36);

        // Configure the SessionId property
        builder.Property(e => e.SessionId)
               .HasColumnName("session_id")
               .HasMaxLength(36);

        // Configure the UserId property
        builder.Property(e => e.UserId)
               .HasColumnName("user_id")
               .HasMaxLength(36);

        // Configure the StartDateTime property
        builder.Property(e => e.StartDateTime)
               .HasColumnName("start_date_time")
               .IsRequired();

        // Configure the EndDateTime property
        builder.Property(e => e.EndDateTime)
               .HasColumnName("end_date_time");

        // Configure the IpAddress property
        builder.Property(e => e.IpAddress)
               .HasColumnName("ip_address")
               .HasMaxLength(45);

        // Configure the UserAgent property
        builder.Property(e => e.UserAgent)
               .HasColumnName("user_agent")
               .HasMaxLength(256);

        // Configure the Status property
        builder.Property(e => e.Status)
               .HasColumnName("status")
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

        builder.ComplexProperty(u => u.EntityDeletionStatus)
               .Property(x => x.DeletedBy)
               .HasColumnName("deleted_by")
               .HasMaxLength(36);

        builder.ComplexProperty(u => u.EntityDeletionStatus)
               .Property(x => x.DeletedOnUtc)
               .HasColumnName("deleted_on_utc")
               .HasMaxLength(36);

        builder.ComplexProperty(u => u.EntityDeletionStatus)
               .Property(x => x.IsDeleted)
               .HasColumnName("is_deleted")
               .IsRequired();
    }
}