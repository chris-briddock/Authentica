using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Results;
using Microsoft.IdentityModel.Tokens;

namespace Application.Providers;

/// <summary>
/// Represents a service for managing JSON Web Tokens.
/// This includes creating, refreshing and validating JSON Web Tokens.
/// </summary>
public class JsonWebTokenProvider : IJsonWebTokenProvider
{
    /// <summary>
    /// Tries to create a JWT (JSON Web Token) asynchronously.
    /// </summary>
    /// <param name="email">The email of the token's recipient.</param>
    /// <param name="jwtSecret">The secret key used to sign the JWT.</param>
    /// <param name="issuer">The issuer of the JWT.</param>
    /// <param name="audience">The intended audience of the JWT.</param>
    /// <param name="expires">The expiration date and time of the JWT.</param>
    /// <param name="subject">The subject of the JWT.</param>
    /// <returns>A <see cref="JwtResult"/> containing the result of the token creation.</returns>
    public async Task<JwtResult> TryCreateTokenAsync(string email,
                                                     string jwtSecret,
                                                     string issuer,
                                                     string audience,
                                                     int expires,
                                                     string subject)
    {
        JwtResult result = new();
        try
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var expiryMinutesToAdd = expires;

            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, subject),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, email)
            ];

            SigningCredentials signingCredentials = new(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512);

            JwtHeader header = new(signingCredentials);

            JwtPayload payload = new(
                issuer: issuer,
                issuedAt: DateTime.UtcNow,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutesToAdd)
            );

            JwtSecurityToken tokenDescriptor = new(header, payload);
            result.Success = true;
            result.Token = tokenHandler.WriteToken(tokenDescriptor);
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
            result.Success = false;
        }

        return await Task.FromResult(result);
    }

    /// <summary>
    /// Tries to validate a JWT (JSON Web Token) asynchronously.
    /// </summary>
    /// <param name="token">The JWT to validate.</param>
    /// <param name="jwtSecret">The secret key used to validate the JWT's signature.</param>
    /// <param name="issuer">The expected issuer of the JWT.</param>
    /// <param name="audience">The expected audience of the JWT.</param>
    /// <returns>A <see cref="JwtResult"/> containing the result of the token validation.</returns>
    public async Task<JwtResult> TryValidateTokenAsync(string token,
                                                       string jwtSecret,
                                                       string issuer,
                                                       string audience)
    {
        JwtResult result = new();
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            TokenValidationResult validationResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);


            // If the validation succeeds, the JWT is both well-formed and has a valid signature.
            if (validationResult.IsValid)
            {
                result.Success = true;
                result.Token = token;
            }

            return result;
        }
        catch (SecurityTokenException ex)
        {
            result.Error = ex.Message;
            result.Success = false;
            throw;
        }
    }
    /// <summary>
    /// Tries to create a new refresh JWT (JSON Web Token) asynchronously.
    /// </summary>
    /// <param name="email">The email of the token's recipient.</param>
    /// <param name="jwtSecret">The secret key used to sign the JWT.</param>
    /// <param name="issuer">The issuer of the JWT.</param>
    /// <param name="audience">The intended audience of the JWT.</param>
    /// <param name="expires">The expiration date and time of the JWT.</param>
    /// <param name="subject">The subject of the JWT.</param>
    /// <returns>A <see cref="JwtResult"/> containing the result of the token creation.</returns>
    public async Task<JwtResult> TryCreateRefreshTokenAsync(string email,
                                                            string jwtSecret,
                                                            string issuer,
                                                            string audience,
                                                            int expires,
                                                            string subject) => await TryCreateTokenAsync(email,
                                                                                                         jwtSecret,
                                                                                                         issuer,
                                                                                                         audience,
                                                                                                         expires,
                                                                                                         subject);

}