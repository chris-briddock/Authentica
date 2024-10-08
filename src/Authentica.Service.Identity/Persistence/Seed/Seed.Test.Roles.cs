using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seed;

public static partial class Seed
{
    public static partial class Test
    {
        /// <summary>
        /// Seeds roles into the database if they don't already exist.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedTestRolesAsync(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            // Create role.
            Role newRole = new()
            {
                Name = "Test",
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityDeletionStatus = new(false, null, null),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            if (!await roleManager.RoleExistsAsync(newRole.Name))
                await roleManager.CreateAsync(newRole);
        }
    }
}