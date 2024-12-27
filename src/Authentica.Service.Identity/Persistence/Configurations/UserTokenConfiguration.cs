using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="IdentityUserToken{TKey}"/>.
/// </summary>
public sealed class UserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    /// <summary>
    /// Configures the entity framework mapping for <see cref="IdentityUserToken{TKey}"/>.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.ToTable("SYSTEM_IDENTITY_USER_TOKENS", opt => opt.IsTemporal());
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
    }
}
