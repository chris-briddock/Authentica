using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configures the properties and relationships of the <see cref="Event"/> entity.
/// </summary>
public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    /// <summary>
    /// Configures the entity of type <see cref="Event"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        // Set the table name and enable temporal table support
        builder.ToTable("SYSTEM_IDENTITY_EVENTS", opt =>
        {
            opt.IsTemporal();
        });

        // Configure the primary key
        builder.HasKey(e => e.Id);

        // Configure the Id property
        builder.Property(e => e.Id)
               .HasColumnName("id")
               .HasMaxLength(36);

        // Configure the sequence id property
        builder.Property(e => e.SequenceId)
               .HasColumnName("sequence_id")
               .HasMaxLength(36);

        // Configure the EventType property
        builder.Property(e => e.EventType)
               .HasColumnName("event_type")
               .HasMaxLength(100);
        
        // Configure the CreatedOn property
        builder.Property(e => e.CreatedOn)
               .HasColumnName("created_on")
               .HasDefaultValueSql("GETUTCDATE()")
               .ValueGeneratedOnAddOrUpdate();
        
        // Configure the Data property
        builder.Property(e => e.Data)
               .HasColumnName("data");
    }
}
