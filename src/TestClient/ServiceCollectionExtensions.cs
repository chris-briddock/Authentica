using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TestClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBearerAuthentication(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            string issuer = "https://localhost:7171";
            string audience = "https://localhost:7171";
            string jwtSecret = "y&>tq_:|8@$u81vM(#kQ;{]|3Adx>m!sSrpkS]iy^Nn|'zjG;s;FDhDLjJEF{/H',FJ~[aJ~qg0$q@!iIeV";

            var key = Encoding.ASCII.GetBytes(jwtSecret);

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

            options.SaveToken = true;

            // Add event handlers for detailed logging
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = async context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(context.Exception, "Authentication failed.");

                    if (!context.Response.HasStarted)
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        var result = System.Text.Json.JsonSerializer.Serialize(new { message = context.Exception.Message });
                        await context.Response.WriteAsync(result);
                    }
                    else
                    {
                        logger.LogWarning("Response has already started, skipping response write.");
                    }
                },
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation("Token validated successfully.");
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogWarning("Token challenge executed.");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}