using Application.Contracts;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.Contexts;

namespace Persistence.Seed;

public static partial class Seed
{
    public static partial class Test
    {
        /// <summary>
        /// Seeds the test client application into the database if it doesn't already exist.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedTestClientApplicationAsync(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var hasher = scope.ServiceProvider.GetRequiredService<ISecretHasher>();

            string appName = "Default Test Application";

            var adminEmail = AdminEmail;
            var user = await userManager.FindByEmailAsync(adminEmail);

            var secret = "eCp79BsVS5uPb7J6MDStjfuw8h1Jv5dSKA89epAtsLy4pyGgJ6IjIfDeibTtXz7uGEMQixQl/XFjfwCUj7esNn0xUkwobzqHVJN43YLZcIZzyV5yLqKKE/Ku/YsVkZqg5/9eMi4jOKsuxGBRbMA9KeNeFk9TYybwXYbpoQTeHg8dvilNy0NsLzcZ9leD9IVmo5hhMmB9n9ghl1U/R6gCjwMaQY8alFntWSnu7SFJkNAv2o6pmaQTFwGQ7b+wl0lTKdASMQZoj/IVlEXwNNz2OOUCUnBTj5rza9ovs5KgyuwsURIBMe6w9DoEBsjtdoqco/o6nNABrmuB66yg==";

            var hashedSecret = hasher.Hash(secret);

            // Check if the initial client application already exists
            var exists = context.ClientApplications.Any(a => a.Name == appName);
            if (!exists)
            {
                ClientApplication application = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = "2e5cf15b-bf5b-4d80-aa01-2a596403530d",
                    Name = appName,
                    CallbackUri = "https://localhost:7256/callback",
                    EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                    EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                    EntityDeletionStatus = new(true, DateTime.UtcNow.AddYears(-8), user!.Id),
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

        /// <summary>
        /// Seeds the initial client application into the database if it doesn't already exist.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedOldDeletedClientApplicationAsync(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var hasher = scope.ServiceProvider.GetRequiredService<ISecretHasher>();
            var stringProvider = scope.ServiceProvider.GetRequiredService<IRandomStringProvider>();

            var adminEmail = AdminEmail;
            var user = await userManager.FindByEmailAsync(adminEmail);

            var secret = stringProvider.GenerateAlphanumeric();

            var hashedSecret = hasher.Hash(secret);

            var appName = "Default Old Deleted Application";

            var exists = context.ClientApplications.Any(a => a.Name == appName);
            if (!exists)
            {
                // Check if the initial client application already exists
                ClientApplication application = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = Guid.NewGuid().ToString(),
                    Name = appName,
                    CallbackUri = "https://localhost:7256/callback",
                    ClientSecret = hashedSecret,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                    EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                    EntityDeletionStatus = new(true, DateTime.UtcNow.AddYears(-8), user!.Id)
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
        /// <summary>
        /// Seeds the initial client application into the database if it doesn't already exist.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedRecentDeletedClientApplicationAsync(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var hasher = scope.ServiceProvider.GetRequiredService<ISecretHasher>();
            var stringProvider = scope.ServiceProvider.GetRequiredService<IRandomStringProvider>();

            var adminEmail = AdminEmail;
            var user = await userManager.FindByEmailAsync(adminEmail);

            var secret = stringProvider.GenerateAlphanumeric();

            var hashedSecret = hasher.Hash(secret);

            var appName = "Default Recent Deleted Application";

            var exists = context.ClientApplications.Any(a => a.Name == appName);
            if (!exists)
            {
                // Check if the initial client application already exists
                ClientApplication application = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = Guid.NewGuid().ToString(),
                    Name = appName,
                    CallbackUri = "https://localhost:7256/callback",
                    ClientSecret = hashedSecret,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                    EntityDeletionStatus = new(true, DateTime.UtcNow, user!.Id),
                    EntityModificationStatus = new(DateTime.UtcNow, CreatedBy)
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
}
