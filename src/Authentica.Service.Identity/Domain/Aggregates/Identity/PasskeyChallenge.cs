using Domain.Contracts;
using Domain.ValueObjects;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a challenge issued during a passkey authentication attempt.
/// </summary>
public sealed class PasskeyChallenge : IEntityCreationStatus<string>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ChallengeId { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public byte[] Challenge { get; set; } = default!;
    public string Status { get; set; }
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;
}
