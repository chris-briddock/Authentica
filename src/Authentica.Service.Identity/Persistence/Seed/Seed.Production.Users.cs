using Api.Constants;
using ChristopherBriddock.AspNetCore.Extensions;

namespace Persistence.Seed;

public static partial class Seed
{
    /// <summary>
    /// Seeds an admin user into the database if it doesn't already exist.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedProdAdminUserAsync(WebApplication app)
    {
        var configuration = app.Services.GetRequiredService<IConfiguration>();
        var email = configuration.GetRequiredValueOrThrow("Defaults:AdminEmail");
        var password = configuration.GetRequiredValueOrThrow("Defaults:AdminPassword");

         await SeedUserAsync(app, email, password, false, false, null, [RoleDefaults.Admin, RoleDefaults.User]);
    }
}