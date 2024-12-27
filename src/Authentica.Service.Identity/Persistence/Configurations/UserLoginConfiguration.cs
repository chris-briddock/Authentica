using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="IdentityUserLogin{TKey}"/>.
/// </summary>
public sealed class UserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    /// <summary>
    /// Configures the entity framework mapping for <see cref="IdentityUserLogin{TKey}"/>.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable("SYSTEM_IDENTITY_USER_LOGIN", opt => opt.IsTemporal());

        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });
    }
}
