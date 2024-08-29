using Domain.Aggregates.Identity;
using Domain.ValueObjects;
using ChristopherBriddock.AspNetCore.Extensions;
using Microsoft.AspNetCore.Identity;
using Api.Constants;

namespace Persistence.Seed;

public static partial class Seed
{
    /// <summary>
    /// Seeds an admin user into the database if it doesn't already exist.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedAdminUserAsync(WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var adminEmail = configuration.GetRequiredValueOrThrow("Defaults:AdminEmail");
        var adminPassword = configuration.GetRequiredValueOrThrow("Defaults:AdminPassword");

        User adminUser = new()
        {
            UserName = adminEmail,
            Email = adminEmail,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            EmailConfirmed = true,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            CreatedBy = CreatedBy,
            Address = new Address(AddressValue, AddressValue, AddressValue, AddressValue, AddressValue, AddressValue, AddressValue)
        };

        // Hash the password for security.
        adminUser.PasswordHash = userManager.PasswordHasher.HashPassword(adminUser, adminPassword);

        var existingUser = await userManager.FindByEmailAsync(adminUser.Email);

        if (existingUser is null)
        {
            await userManager.CreateAsync(adminUser);
            // Add roles to the admin user.
            await userManager.AddToRoleAsync(adminUser, RoleDefaults.Admin);
            await userManager.AddToRoleAsync(adminUser, RoleDefaults.User);
        }
    }
}