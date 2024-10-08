using Api.Requests;
using Api.Responses;

namespace Application.Activities.Users;

/// <summary>
/// Represents an activity for managing mfa authenticator settings.
/// </summary>
public class MultiFactorManageAuthenticatorActivity : ActivityBase<MultiFactorManageAuthenticatorRequest, MultiFactorManageAuthenticatorResponse>
{
    /// <summary>
    /// Gets or sets the request containing information for managing the authenticator.
    /// </summary>
    public override MultiFactorManageAuthenticatorRequest Request { get; set; } = default!;

    /// <summary>
    /// Gets or sets the response containing the result of the authenticator management operation.
    /// </summary>
    public override MultiFactorManageAuthenticatorResponse Response { get; set; } = default!;
}
