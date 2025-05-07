using Domain.Contracts;
using Domain.ValueObjects;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a default implementation of multi-factor authentication (MFA) settings for a user.
/// </summary>
public sealed class UserMultiFactorSettings : UserMultiFactorSettings<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the MFA settings.
    /// </summary>
    /// <remarks>This identifier is used to uniquely identify the MFA settings for a user.</remarks>
    public override string Id { get; set; } = Gusid.New().ToString();

    /// <summary>
    /// Gets or sets the foreign key for the user.
    /// </summary>
    /// <remarks>This foreign key is used to link the MFA settings to the corresponding user.</remarks>
    public string UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user associated with these MFA settings.
    /// </summary>
    /// <remarks>This property provides a reference to the user object associated with these MFA settings.</remarks>
    public User User { get; set; } = default!;

}
/// <summary>
/// Represents the base class for multi-factor authentication (MFA) settings.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the MFA settings.</typeparam>
/// <remarks>This class provides a base implementation for MFA settings and can be inherited to create custom MFA settings classes.</remarks>
public abstract class UserMultiFactorSettings<TKey> : 
IEntityCreationStatus<TKey>,
IEntityModificationStatus<TKey>
where TKey : IEquatable<TKey> 
{
    /// <summary>
    /// Gets or sets the unique identifier for the MFA settings.
    /// </summary>
    /// <remarks>This identifier is used to uniquely identify the MFA settings.</remarks>
    public virtual TKey Id { get; set; } = default!;
    /// <summary>
    /// Gets or sets a flag indicating if MFA via email is enabled.
    /// </summary>
    /// <remarks>This flag determines whether MFA via email is enabled for the user.</remarks>
    public virtual bool MultiFactorEmailEnabled { get; set; } = default!;
    /// <summary>
    /// Gets or sets if the user has enabled application-based TOTPs.
    /// </summary>
    /// <remarks>This flag determines whether the user has enabled application-based TOTPs for MFA.</remarks>
    public virtual bool MultiFactorAuthenticatorEnabled { get; set; } = default!;
    /// <summary>
    /// Gets or sets if the user has enabled passkeys.
    /// </summary>
    /// <remarks>This flag determines whether the user has enabled passkeys for MFA.</remarks>
    public virtual bool MultiFactorPasskeysEnabled { get; set; } = default!;

    /// <summary>
    /// A random value that should change whenever the entity is persisted.
    /// </summary>
    public virtual string ConcurrencyStamp { get; set; } = Gusid.New().ToString();
    /// <summary>
    /// Gets or sets the creation status of the entity.
    /// </summary>
    /// <remarks>This property provides information about the creation status of the entity.</remarks>
    public EntityCreationStatus<TKey> EntityCreationStatus { get; set; } = default!;
    /// <summary>
    /// Gets or sets the modification status of the entity.
    /// </summary>
    /// <remarks>This property provides information about the modification status of the entity.</remarks>
    public EntityModificationStatus<TKey> EntityModificationStatus { get; set; } = default!;
}
