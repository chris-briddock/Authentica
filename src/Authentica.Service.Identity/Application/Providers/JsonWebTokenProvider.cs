using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts;
using Application.Results;
using Microsoft.IdentityModel.Tokens;

namespace Application.Providers;

/// <summary>
/// Represents a service for managing JSON Web Tokens.
/// This includes creating, refreshing and validating JSON Web Tokens.
/// </summary>
public sealed class JsonWebTokenProvider : IJsonWebTokenProvider
{
    /// <inheritdoc/>
    public async Task<JwtResult> TryCreateTokenAsync(string email,
                                                     string jwtSecret,
                                                     string issuer,
                                                     string audience,
                                                     int expires,
                                                     string subject,
                                                     IList<string> roles,
                                                     IList<string> scopes)
    {
        JwtResult result = new();
        try
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSecret);
            var expiryMinutesToAdd = expires;

            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, subject),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, email)
            ];

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            foreach (var scope in scopes)
            {
                claims.Add(new Claim("scp", scope));
            }

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

    /// <inheritdoc/>
    public async Task<JwtResult> TryValidateTokenAsync(string token,
                                                       string jwtSecret,
                                                       string issuer,
                                                       string audience)
    {
        JwtResult result = new();
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSecret);
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
    /// <inheritdoc/>
    public async Task<JwtResult> TryCreateRefreshTokenAsync(string email,
                                                            string jwtSecret,
                                                            string issuer,
                                                            string audience,
                                                            int expires,
                                                            string subject,
                                                            IList<string> roles,
                                                            IList<string> scopes) => await TryCreateTokenAsync(email,
                                                                                                                jwtSecret,
                                                                                                                issuer,
                                                                                                                audience,
                                                                                                                expires,
                                                                                                                subject,
                                                                                                                roles,
                                                                                                                scopes);

}