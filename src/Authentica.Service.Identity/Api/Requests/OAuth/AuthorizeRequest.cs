using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Requests;

/// <summary>
/// Represents an OAuth 2.0 authorization request.
/// </summary>
public sealed record AuthorizeRequest
{
    /// <summary>
    /// Gets or sets the client identifier issued to the client during the registration process.
    /// </summary>
    [FromQuery(Name = "client_id")]
    [ProtectedPersonalData]
    public string ClientId { get; init; } = default!;

    /// <summary>
    /// Gets or sets the URI to which the response will be sent. 
    /// This value must match one of the redirection URI values registered during client registration.
    /// </summary>
    [FromQuery(Name = "callback_uri")]
    public string CallbackUri { get; init; } = default!;

    /// <summary>
    /// Gets or sets the scope of the access request. 
    /// This is a space-delimited list of strings.
    /// </summary>
    [FromQuery(Name = "scope")]
    public string? Scope { get; init; } = default!;

    /// <summary>
    /// Gets or sets the response type, which determines the authorization processing flow to be used.
    /// Typical values are "code" for the authorization code flow, "client_credentials" for the client credentials flow.
    /// </summary>
    [FromQuery(Name = "response_type")]
    public string ResponseType { get; init; } = default!;

    /// <summary>
    /// Gets or sets the code challenge used in the PKCE (Proof Key for Code Exchange) extension to OAuth 2.0.
    /// This is relevant for the authorization code flow.
    /// </summary>
    [FromQuery(Name = "code_challenge")]
    public string? CodeChallenge { get; init; }

    /// <summary>
    /// Gets or sets the method used to derive the code challenge, typically "plain" or "S512".
    /// This is relevant for the authorization code flow.
    /// </summary>
    [FromQuery(Name = "code_challenge_method")]
    public string? CodeChallengeMethod { get; init; }
}
