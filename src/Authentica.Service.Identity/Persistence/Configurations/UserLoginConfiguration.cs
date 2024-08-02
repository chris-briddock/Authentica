using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

/// <summary>
/// Configuration class for the entity framework mapping of <see cref="IdentityUserLogin{TKey}"/>.
/// </summary>
public class UserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable("SYSTEM_IDENTITY_USER_LOGIN", opt => 
        {
            opt.IsTemporal();
        });
        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });
    }
}
