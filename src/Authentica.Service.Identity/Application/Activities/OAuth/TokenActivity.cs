using Api.Requests;
using Api.Responses;

namespace Application.Activities;

/// <summary>
/// Represents an event that occurs when a token request is made.
/// </summary>
public sealed class TokenActivity : ActivityBase<TokenRequest, TokenResponse>
{
    /// <summary>
    /// Gets or sets the request data for the token event.
    /// </summary>
    public override TokenRequest Request { get; set; } = default!;

    /// <summary>
    /// Gets or sets the response data for the token event.
    /// </summary>
    public override TokenResponse Response { get; set; } = default!;
}
