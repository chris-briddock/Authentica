namespace Application.Extensions;

/// <summary>
/// Provides extension methods for HttpContext.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Gets the IP address of the client making the request.
    /// </summary>
    /// <param name="context">The HttpContext.</param>
    /// <returns>
    /// A string representing the IP address of the client.
    /// Returns null if the IP address cannot be determined.
    /// </returns>
    /// <remarks>
    /// This method first checks the X-Forwarded-For header, which is typically set by proxies or load balancers.
    /// If that header is not present, it falls back to the RemoteIpAddress from the connection.
    /// Be aware that IP addresses can be spoofed, so this should not be used as a sole means of identification.
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