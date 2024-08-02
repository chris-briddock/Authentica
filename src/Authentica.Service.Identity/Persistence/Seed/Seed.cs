using Api.Constants;
using Application.Contracts;
using Authentica.Service.Identity;
using Domain.Aggregates.Identity;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Persistence.Contexts;

namespace Persistence.Seed;

/// <summary>
/// Provides methods for seeding initial data into the application database.
/// </summary>
public static class Seed
{
    /// <summary>
    /// Seeds the initial client application into the database if it doesn't already exist.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedClientApplicationAsync(WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var hasher = scope.ServiceProvider.GetRequiredService<ISecretHasher>();
        var stringProvider = scope.ServiceProvider.GetRequiredService<IRandomStringProvider>();
        
        var adminEmail = "admin@default.com";
        var user = await userManager.FindByEmailAsync(adminEmail);

        var secret = "eCp+79BsVS5uPb7J6MD+Stjfuw8h1Jv5dSKA89ep+AtsLy4+pyGgJ6IjIfDeibTtXz7uGEMQixQl/XFjfwCUj+7esNn0xUkwobzqHVJN43YLZcI+ZzyV5yLqKKE/Ku/YsVkZqg5/9eMi4jOKsuxGBRbMA9KeNeFk9TYybwXYbpoQTeHg8dvilNy0NsLzcZ9leD9IVmo5hhMmB9n9ghl1U/R6gCjwMaQY8alFntWSnu7SFJkNAv2o6pmaQTFwGQ7b+wl0lTKdASMQZoj/IVlEXwNNz2OOUCUnBTj5rza9ovs5KgyuwsURIBMe6w9DoEBsjtdoqco/o6nNABrmuB66yg==";

        var hashedSecret = hasher.Hash(secret);

        // Check if the initial client application already exists
        if (!context.ClientApplications.Any())
        {
            ClientApplication application = new()
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = "2e5cf15b-bf5b-4d80-aa01-2a596403530d",
                Name = "Default Application",
                CallbackUri = "https://localhost:7256/callback",
                RedirectUri = "https://localhost:7256",
                CreatedBy = "SYSTEM",
                ClientSecret = hashedSecret,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            ICollection<UserClientApplication> linkTableCollection = 
            [ 
                new UserClientApplication()
                {
                    UserId = user!.Id,
                    ApplicationId = application.Id
                }
            ];

            application.UserClientApplications = linkTableCollection;

            context.ClientApplications.Add(application);
            await context.SaveChangesAsync();
            
            logger.LogInformation("Initial application client secret: {secret}", secret);
        }
    }
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
                CreatedBy = "SYSTEM",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            if (!await roleManager.RoleExistsAsync(newRole.Name))
            {
                await roleManager.CreateAsync(newRole);
            }
        }
    }
    /// <summary>
    /// Seeds an admin user into the database if it doesn't already exist.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAdminUserAsync(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var adminEmail = "admin@default.com";

            User adminUser = new()
            {
                UserName = adminEmail,
                Email = adminEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                CreatedBy = "SYSTEM",
                Address = new Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
            };

            // Hash the password for security.
            adminUser.PasswordHash = userManager.PasswordHasher.HashPassword(adminUser, "fR<pGWqvn4Mu,6w[Z8axP;b5=");
            
            var existingUser = await userManager.FindByEmailAsync(adminUser.Email);

            if (existingUser is null)
            {
                await userManager.CreateAsync(adminUser);
                // Add roles to the admin user.
                await userManager.AddToRoleAsync(adminUser, RoleDefaults.Admin);
                await userManager.AddToRoleAsync(adminUser, RoleDefaults.User);
            }
                
        }
    /// <summary>
    /// Seeds all test user data.
    /// </summary>
    public static class Test 
    {
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

            var adminEmail = "admin@default.com";
            var user = await userManager.FindByEmailAsync(adminEmail);

            var secret = stringProvider.Generate();

            var hashedSecret = hasher.Hash(secret);

            // Check if the initial client application already exists
            if (!context.ClientApplications.Any())
            {
                ClientApplication application = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = Guid.NewGuid().ToString(),
                    Name = "Default Old Deleted Application",
                    CallbackUri = "https://localhost:7256/callback",
                    RedirectUri = "https://localhost:7256",
                    CreatedBy = "SYSTEM",
                    ClientSecret = hashedSecret,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    DeletedBy = user!.Id,
                    IsDeleted = true,
                    DeletedOnUtc = DateTime.UtcNow.AddYears(-8)

                };

                ICollection<UserClientApplication> linkTableCollection =
                [
                    new UserClientApplication()
                {
                    UserId = user!.Id,
                    ApplicationId = application.Id
                }
                ];

                application.UserClientApplications = linkTableCollection;

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

            var adminEmail = "admin@default.com";
            var user = await userManager.FindByEmailAsync(adminEmail);

            var secret = stringProvider.Generate();

            var hashedSecret = hasher.Hash(secret);

            // Check if the initial client application already exists
            ClientApplication application = new()
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = Guid.NewGuid().ToString(),
                Name = "Default Recent Deleted Application",
                CallbackUri = "https://localhost:7256/callback",
                RedirectUri = "https://localhost:7256",
                CreatedBy = "SYSTEM",
                ClientSecret = hashedSecret,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                DeletedBy = user!.Id,
                IsDeleted = true,
                DeletedOnUtc = DateTime.UtcNow

            };

            ICollection<UserClientApplication> linkTableCollection =
                [
                    new UserClientApplication()
                {
                    UserId = user!.Id,
                    ApplicationId = application.Id
                }
                ];

            application.UserClientApplications = linkTableCollection;

                context.ClientApplications.Add(application);
                await context.SaveChangesAsync();
        }
        /// <summary>
        /// Seeds an deleted user into the database if it doesn't already exist.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedDeletedUser(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var userEmail = "deletedUser@default.com";

            User user = new()
            {
                UserName = userEmail,
                Email = userEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = true,
                DeletedOnUtc = DateTime.UtcNow,
                EmailConfirmed = true,
                CreatedBy = "DEFAULT",
                Address = new Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
            };

            // Hash the password for security.
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "fR<pGW'qvn4Mu,6w[Z8axP;b5=");
            
            var existingUser = await userManager.FindByEmailAsync(user.Email);

            if (existingUser is null)
            {
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, RoleDefaults.User);
            }
            

            
        }
        /// <summary>
        /// Seeds a user into the database if it doesn't already exist for testing 
        /// the authorize endpoint.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAuthorizeUser(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var userEmail = "authorizeTest@default.com";

            User user = new()
            {
                UserName = userEmail,
                Email = userEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = false,
                DeletedOnUtc = default!,
                EmailConfirmed = true,
                Address = new Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT"),
                CreatedBy = "SYSTEM"
            };

            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`");

            var existingUser = await userManager.FindByEmailAsync(user.Email);
            if (existingUser is null)
            {
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, RoleDefaults.User);
            }
        }

        /// <summary>
        /// Seeds a user into the database if it doesn't already exist for testing 
        /// the authorize endpoint.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedTwoFactorUser(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var userEmail = "twoFactorTest@default.com";

            User user = new()
            {
                UserName = userEmail,
                Email = userEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = false,
                DeletedOnUtc = default!,
                EmailConfirmed = true,
                CreatedBy = "SYSTEM",
                Address = new Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
            };

            // Hash the password for security.
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "Ar*P`w8R.WyXb7'UKxh;!-");

            var existingUser = await userManager.FindByEmailAsync(user.Email);
            if (existingUser is null)
            {
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, RoleDefaults.User);
            }
        }
         /// <summary>
        /// Seeds two test users into the database if it doesn't already exist for testing
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedBackgroundServiceUsers(WebApplication app) 
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var oldDeletedUserEmail = "olddeleted@default.com";
            var recentDeletedUserEmail = "recentlydeleted@default.com";

            var oldDeletedUser = new User()
            {
                UserName = oldDeletedUserEmail,
                Email = oldDeletedUserEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = true,
                DeletedOnUtc = DateTime.UtcNow.AddYears(-8),
                EmailConfirmed = true,
                CreatedBy = "SYSTEM",
                Address = new Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
                
            };

            oldDeletedUser.PasswordHash = userManager.PasswordHasher.HashPassword(oldDeletedUser, "dnjdnjdnwjdnwqjdnqwj");

            var recentDeletedUser = new User
            {
                UserName = recentDeletedUserEmail,
                Email = recentDeletedUserEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = true,
                EmailConfirmed = true,
                CreatedBy = "SYSTEM",
                Address = new Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
                
            };
            recentDeletedUser.PasswordHash = userManager.PasswordHasher.HashPassword(recentDeletedUser, "dnjdnjdnwjdnwqjdnqwj");

            var oldDeletedUserExists = await userManager.FindByEmailAsync(oldDeletedUserEmail);
            var recentDeletedUserExists = await userManager.FindByEmailAsync(recentDeletedUserEmail);

            if (oldDeletedUserExists is null &&
                recentDeletedUserExists is null)
            {
                await userManager.CreateAsync(oldDeletedUser);
                await userManager.CreateAsync(recentDeletedUser);

                await userManager.AddToRoleAsync(oldDeletedUser, RoleDefaults.User);
                await userManager.AddToRoleAsync(recentDeletedUser, RoleDefaults.User);
            }
        }
    }
}