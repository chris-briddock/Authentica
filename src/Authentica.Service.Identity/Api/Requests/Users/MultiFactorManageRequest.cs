using Microsoft.AspNetCore.Mvc;

namespace Api.Requests;

/// <summary>
/// Represent a users request to enable or disable mfa.
/// </summary>
public sealed record MultiFactorManageRequest
{
    /// <summary>
    /// Gets or sets the isEnabled flag.
    /// </summary>
    [FromQuery(Name = "is_enabled")]
    public bool IsEnabled { get; set; } = default!;
}
