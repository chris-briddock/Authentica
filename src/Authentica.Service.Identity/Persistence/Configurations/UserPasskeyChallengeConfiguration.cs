using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration for <see cref="UserPasskeyChallenge"/>
/// </summary>
public class UserPasskeyChallengeConfiguration : IEntityTypeConfiguration<UserPasskeyChallenge>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserPasskeyChallenge> builder)
    {
        builder.ToTable("SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE", opt => opt.IsTemporal());

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasColumnName("id")
               .HasMaxLength(36);

        builder.Property(u => u.UserId)
               .HasColumnName("user_id")
               .HasMaxLength(36);

        builder.Property(u => u.PasskeyChallengeId)
               .HasColumnName("passkey_challenge_id")
               .HasMaxLength(36);

        builder.HasOne(u => u.User)
               .WithMany(upc => upc.UserPasskeyChallenges)
               .HasForeignKey(upc => upc.UserId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
