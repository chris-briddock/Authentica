using System.Security.Claims;

namespace Application.DTOs;

/// <summary>
/// Represents a data transfer object for an application.
/// </summary>
public class ApplicationDto<TRequest>
where TRequest : class
{
    /// <summary>
    /// Gets or sets the request data for the client application.
    /// </summary>
    public TRequest Request { get; set; } = default!;

    /// <summary>
    /// Gets or sets the claims principal representing the current user.
    /// </summary>
    public ClaimsPrincipal ClaimsPrincipal { get; set; } = default!;
}
