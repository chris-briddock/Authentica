using System.Text;
using Application.Contracts;
using Application.Providers;
using ChristopherBriddock.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Application.Extensions;

public static partial class ServiceCollectionExtensions 
{
    
    /// <summary>
    /// Adds bearer authentication services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddBearerAuthentication(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.TryAddScoped<IJsonWebTokenProvider, JsonWebTokenProvider>();
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => 
        {
            // Retrieve the JWT secret from the application configuration
            string issuer = configuration.GetRequiredValueOrThrow("Jwt:Issuer");
            string audience = configuration.GetRequiredValueOrThrow("Jwt:Audience");
            string jwtSecret = configuration.GetRequiredValueOrThrow("Jwt:Secret");

            var key = Encoding.ASCII.GetBytes(jwtSecret);
            // Configure token validation parameters
            options.TokenValidationParameters = new TokenValidationParameters()
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

            // Save the token in the authentication context
            options.SaveToken = true;
        });

        return services;
    }
}