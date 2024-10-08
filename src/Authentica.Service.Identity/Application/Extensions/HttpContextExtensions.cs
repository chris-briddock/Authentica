namespace Application.Extensions;

/// <summary>
/// Provides extension methods for HttpContext.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Gets the client's IP address from the request.
    /// </summary>
    /// <param name="context">The HttpContext.</param>
    /// <returns>Client's IP address or null if not determined.</returns>
    /// <remarks>
    /// Checks X-Forwarded-For header first, then falls back to RemoteIpAddress.
    /// Note: IP addresses can be spoofed.
    /// </remarks>
    public static string GetIpAddress(this HttpContext context)
    {
        // Check for X-Forwarded-For header
        string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()!;
        if (!string.IsNullOrEmpty(ip))
        {
            // X-Forwarded-For may contain multiple IP addresses; take the first one
            return ip.Split(',')[0].Trim();
        }

        // If X-Forwarded-For is not present, use RemoteIpAddress
        return context.Connection.RemoteIpAddress?.ToString()!;
    }
}