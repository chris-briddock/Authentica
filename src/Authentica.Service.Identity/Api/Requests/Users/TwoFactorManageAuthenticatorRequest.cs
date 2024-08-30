namespace Api.Requests;

public sealed record TwoFactorManageAuthenticatorRequest
{
    public bool IsEnabled { get; init; } = false;
}