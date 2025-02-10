using Api.Constants;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seed;

public static partial class Seed
{
    public static async Task SeedProductionRoles(WebApplication app) => 
        await SeedRolesAsync(app, [RoleDefaults.Admin, RoleDefaults.User]);
}
