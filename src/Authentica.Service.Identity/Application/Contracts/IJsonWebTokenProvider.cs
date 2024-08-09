using Application.Results;

namespace Application.Providers;

/// <summary>
/// Defines a contract for the JWT Token Provider implementation <see cref="JsonWebTokenProvider"/>
/// </summary>
public interface IJsonWebTokenProvider
{
    /// <summary>
    /// Tries to create a refresh JWT (JSON Web Token) asynchronously.
    /// </summary>
    /// <param name="email">The email of the token's recipient.</param>
    /// <param name="jwtSecret">The secret key used to sign the JWT.</param>
    /// <param name="issuer">The issuer of the JWT.</param>
    /// <param name="audience">The intended audience of the JWT.</param>
    /// <param name="expires">The expiration date and time of the JWT.</param>
    /// <param name="subject">The subject of the JWT.</param>
    /// <param name="roles">The roles that are stored as claims in the JWT.</param>
    /// <returns>A <see cref="JwtResult"/> containing the result of the token creation.</returns>
    Task<JwtResult> TryCreateRefreshTokenAsync(string email,
                                               string jwtSecret,
                                               string issuer,
                                               string audience,
                                               int expires,
                                               string subject,
                                               IList<string> roles);
    /// <summary>
    /// Tries to create a new JWT (JSON Web Token) asynchronously.
    /// </summary>
    /// <param name="email">The email of the token's recipient.</param>
    /// <param name="jwtSecret">The secret key used to sign the JWT.</param>
    /// <param name="issuer">The issuer of the JWT.</param>
    /// <param name="audience">The intended audience of the JWT.</param>
    /// <param name="expires">The expiration date and time of the JWT.</param>
    /// <param name="subject">The subject of the JWT.</param>
    /// <param name="roles">The roles that are stored as claims in the JWT.</param>
    /// <returns>A <see cref="JwtResult"/> containing the result of the token creation.</returns>
    Task<JwtResult> TryCreateTokenAsync(string email,
                                        string jwtSecret,
                                        string issuer,
                                        string audience,
                                        int expires,
                                        string subject,
                                        IList<string> roles);
    /// <summary>
    /// Tries to validate a JWT (JSON Web Token) asynchronously.
    /// </summary>
    /// <param name="token">The JWT to validate.</param>
    /// <param name="jwtSecret">The secret key used to validate the JWT's signature.</param>
    /// <param name="issuer">The expected issuer of the JWT.</param>
    /// <param name="audience">The expected audience of the JWT.</param>
    /// <returns>A <see cref="JwtResult"/> containing the result of the token validation.</returns>
    Task<JwtResult> TryValidateTokenAsync(string token,
                                          string jwtSecret,
                                          string issuer,
                                          string audience);
}