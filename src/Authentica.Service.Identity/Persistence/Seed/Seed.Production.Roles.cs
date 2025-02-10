using Api.Constants;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seed;

public static partial class Seed
{
    /// <summary>
    /// Seeds the production roles.
    /// </summary>
    /// <param name="app">The web application instance used to seed roles.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SeedProdRoles(WebApplication app) => 
        await SeedRolesAsync(app, [RoleDefaults.Admin, RoleDefaults.User]);
}
