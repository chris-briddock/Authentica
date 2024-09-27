using Api.Requests;
using Api.Responses;

namespace Application.Activities.Users;

/// <summary>
/// Represents an activity for managing two-factor authentication authenticator settings.
/// </summary>
public class TwoFactorManageAuthenticatorActivity : ActivityBase<TwoFactorManageAuthenticatorRequest, TwoFactorManageAuthenticatorResponse>
{
    /// <summary>
    /// Gets or sets the request containing information for managing the authenticator.
    /// </summary>
    public override TwoFactorManageAuthenticatorRequest Request { get; set; } = default!;

    /// <summary>
    /// Gets or sets the response containing the result of the authenticator management operation.
    /// </summary>
    public override TwoFactorManageAuthenticatorResponse Response { get; set; } = default!;
}
