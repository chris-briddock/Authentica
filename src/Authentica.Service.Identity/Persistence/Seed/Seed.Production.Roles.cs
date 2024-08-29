using Api.Constants;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seed;

public static partial class Seed
{
    /// <summary>
    /// Seeds roles into the database if they don't already exist.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedRolesAsync(WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        string[] roles = [RoleDefaults.Admin, RoleDefaults.User];

        // Create roles.
        foreach (var role in roles)
        {
            Role newRole = new()
            {
                Name = role,
                NormalizedName = role.ToUpper(),
                CreatedBy = CreatedBy,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            if (!await roleManager.RoleExistsAsync(newRole.Name))
                await roleManager.CreateAsync(newRole);
        }
    }
}   
