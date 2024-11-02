using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configures the properties and relationships of the <see cref="Activity"/> entity.
/// </summary>
public sealed class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    /// <summary>
    /// Configures the entity of type <see cref="Activity"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        // Set the table name and enable temporal table support
        builder.ToTable("SYSTEM_IDENTITY_ACTIVITIES", opt =>
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

        // Configure the ActivityType property
        builder.Property(e => e.ActivityType)
               .HasColumnName("activity_type")
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
