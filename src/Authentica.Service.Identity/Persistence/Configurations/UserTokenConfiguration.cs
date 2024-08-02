using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="IdentityUserToken{TKey}"/>.
/// </summary>
public class UserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.ToTable("SYSTEM_IDENTITY_USER_TOKENS", opt => opt.IsTemporal());
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
    }
}
