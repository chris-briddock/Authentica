using Domain.Contracts;
using Domain.ValueObjects;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a temporary challenge used in the FIDO2 passkey authentication or registration process.
/// </summary>
public sealed class PasskeyChallenge : IEntityCreationStatus<string>
{
    /// <summary>
    /// A unique identifier for the passkey challenge.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// A unique identifier for the challenge, associated with a specific authentication or registration request.
    /// </summary>
    public string ChallengeId { get; set; } = default!;

    /// <summary>
    /// The cryptographic challenge data that will be signed by the client's authenticator.
    /// </summary>
    public byte[] Challenge { get; set; } = default!;

    /// <summary>
    /// The current status of the challenge (e.g., "Pending", "Completed", or "Expired").
    /// </summary>
    public string Status { get; set; } = default!;

    /// <summary>
    /// The date and time when the challenge expires and is no longer valid.
    /// </summary>
    public DateTime ExpiresAt { get; set; } = default!;

    /// <summary>
    /// Metadata representing the creation and tracking status of the entity.
    /// </summary>
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;

    /// <summary>
    /// Represents that the many to one relationship between UserPasskeyChallenge and User.
    /// </summary>
    public ICollection<UserPasskeyChallenge> UserPasskeyChallenges { get; set; } = default!;
}
