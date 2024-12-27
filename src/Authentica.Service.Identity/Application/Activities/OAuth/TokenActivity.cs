using Domain.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an event that occurs when a token request is made.
/// </summary>
public sealed class TokenActivity : ActivityBase<TokenRequest>
{
    /// <summary>
    /// Gets or sets the request data for the token event.
    /// </summary>
    public override TokenRequest Payload { get; set; } = default!;
}
