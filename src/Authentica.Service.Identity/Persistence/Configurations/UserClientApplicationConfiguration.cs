using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="UserClientApplication"/>
/// </summary>
public class UserClientApplicationConfiguration : IEntityTypeConfiguration<UserClientApplication>
{
    /// <summary>
    /// Configures the entity framework mapping for <see cref="UserClientApplication"/>.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<UserClientApplication> builder)
    {
        builder.ToTable("SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS", opt => 
        {
            opt.IsTemporal();
        });
        
        builder.HasKey(uca => uca.Id);

        builder.Property(uca => uca.Id)
                .HasMaxLength(36);

        builder.Property(uca => uca.UserId)
                .HasColumnName("user_id")
               .HasMaxLength(36)
               .IsRequired();

        builder.Property(uca => uca.ApplicationId)
               .HasColumnName("application_id")
               .HasMaxLength(36)
               .IsRequired();

        // Configure the relationship with User
        builder.HasOne(uc => uc.User)
            .WithMany(u => u.UserClientApplications)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the relationship with ClientApplication
        builder.HasOne(uc => uc.Application)
            .WithMany(a => a.UserClientApplications)
            .HasForeignKey(uc => uc.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}