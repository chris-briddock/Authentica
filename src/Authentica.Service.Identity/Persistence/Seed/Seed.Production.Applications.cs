using Application.Contracts;
using ChristopherBriddock.AspNetCore.Extensions;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.Contexts;

namespace Persistence.Seed;

public static partial class Seed
{
    /// <summary>
    /// Seeds the client application into the database if it doesn't already exist.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedClientApplicationAsync(WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var hasher = scope.ServiceProvider.GetRequiredService<ISecretHasher>();

        var adminEmail = configuration.GetRequiredValueOrThrow("Defaults:AdminEmail");
        var secret = configuration.GetRequiredValueOrThrow("Defaults:Secret");
        var callbackUri = configuration.GetRequiredValueOrThrow("Defaults:CallbackUri");
        var user = await userManager.FindByEmailAsync(adminEmail);

        var hashedSecret = hasher.Hash(secret);

        // Check if the initial client application already exists
        if (!context.ClientApplications.Any())
        {
            ClientApplication application = new()
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = Guid.NewGuid().ToString(),
                Name = "Authentica Default Application",
                CallbackUri = $"{callbackUri}",
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityDeletionStatus = new(false, null, null),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                ClientSecret = hashedSecret,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            application.UserClientApplications =
            [
                new UserClientApplication()
                {
                    UserId = user!.Id,
                    ApplicationId = application.Id
                }
            ];

            context.ClientApplications.Add(application);
            await context.SaveChangesAsync();
        }
    }
}