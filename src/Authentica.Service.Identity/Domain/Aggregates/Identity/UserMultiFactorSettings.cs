namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a default implementation of multi-factor authentication (MFA) settings for a user.
/// </summary>
public sealed class UserMultiFactorSettings : UserMultiFactorSettings<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the MFA settings.
    /// </summary>
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the foreign key for the user.
    /// </summary>
    public string UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user associated with these MFA settings.
    /// </summary>
    public User User { get; set; } = default!;

}
/// <summary>
/// Represents the base class for multi-factor authentication (MFA) settings.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the MFA settings.</typeparam>
public abstract class UserMultiFactorSettings<TKey> where TKey : class
{
    /// <summary>
    /// Gets or sets the unique identifier for the MFA settings.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;
    /// <summary>
    /// Gets or sets a flag indicating if mfa via email is enabled.
    /// </summary>
    public virtual bool MultiFactorEmailEnabled { get; set; } = default!;

    /// <summary>
    /// Gets or sets if the user has enabled application-based TOTPs.
    /// </summary>
    public virtual bool MultiFactorAuthenticatorEnabled { get; set; } = default!;
    /// <summary>
    /// Gets or sets if the user has enabled passkeys.
    /// </summary>
    public virtual bool MultiFactorPasskeysEnabled { get; set; } = default!;

}
