namespace Api.Responses;

public sealed record TwoFactorManageAuthenticatorResponse
{
    public string? AuthenticatorKey { get; set; }
    public string? QrCodeUri { get; set; }
}