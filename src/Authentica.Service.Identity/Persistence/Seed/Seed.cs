using System.Globalization;
using Api.Constants;
using Domain.Aggregates.Identity;
using Domain.Contracts.Cryptography;
using Domain.Contracts.Providers;
using Domain.Contracts.Stores;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Persistence.Contexts;

namespace Persistence.Seed;

/// <summary>
/// Provides methods for seeding initial data into the application database.
/// </summary>
public static partial class Seed
{
    private static string[] AdminRoles { get; set; } = [RoleDefaults.Admin, RoleDefaults.User];
    private static string[] UserRoles { get; set; } = [RoleDefaults.User];
    /// <summary>
    /// Pre defined secret for the test client application.
    /// </summary>
    public const string Secret = "eCp79BsVS5uPb7J6MDStjfuw8h1Jv5dSKA89epAtsLy4pyGgJ6IjIfDeibTtXz7uGEMQixQl/XFjfwCUj7esNn0xUkwobzqHVJN43YLZcIZzyV5yLqKKE/Ku/YsVkZqg5/9eMi4jOKsuxGBRbMA9KeNeFk9TYybwXYbpoQTeHg8dvilNy0NsLzcZ9leD9IVmo5hhMmB9n9ghl1U/R6gCjwMaQY8alFntWSnu7SFJkNAv2o6pmaQTFwGQ7b+wl0lTKdASMQZoj/IVlEXwNNz2OOUCUnBTj5rza9ovs5KgyuwsURIBMe6w9DoEBsjtdoqco/o6nNABrmuB66yg==";
    /// <summary>
    /// Default value for created by.
    /// </summary>
    public const string CreatedBy = "SYSTEM";
    /// <summary>
    /// Default value for an address.
    /// </summary>
    public const string AddressValue = "DEFAULT";

    /// <summary>
    /// Default admin email value for test data.
    /// </summary>
    public const string AdminEmail = "admin@default.com";

    /// <summary>
    /// Seeds a client application into the database.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <param name="appName">The name of the client application to seed.</param>
    /// <param name="secret">The application secret</param>
    /// <param name="clientId">The unique identifier for the client application.</param>
    /// <param name="isDeleted">Indicates whether the client application is marked as deleted.</param>
    /// <param name="deletedAt">The date and time when the client application was deleted, if applicable.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private static async Task SeedClientApplicationAsync(WebApplication app,
                                                         string appName,
                                                         bool isDeleted,
                                                         string secret,
                                                         string clientId,
                                                         DateTime? deletedAt = null)
    {
        using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var hasher = scope.ServiceProvider.GetRequiredService<ISecretHasher>();

        var adminEmail = AdminEmail;
        var user = await userManager.FindByEmailAsync(adminEmail);
        if (user is null)
        {
            return;
        }

        var hashedSecret = hasher.Hash(secret);

        if (!context.ClientApplications.Any(a => a.Name == appName))
        {
            var application = new ClientApplication
            {
                Id = Ulid.NewUlid().ToString(),
                ClientId = clientId,
                Name = appName,
                CallbackUri = "https://localhost:7256/callback",
                ClientSecret = hashedSecret,
                ConcurrencyStamp = Ulid.NewUlid().ToString(),
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityDeletionStatus = new(isDeleted, deletedAt, isDeleted ? user.Id : null)
            };

            application.UserClientApplications =
            [
                new UserClientApplication
                    {
                        UserId = user.Id,
                        ApplicationId = application.Id
                    }
            ];

            context.ClientApplications.Add(application);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Seeds a user into the database if they do not already exist.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="twoFactorEnabled">Indicates if two-factor authentication is enabled.</param>
    /// <param name="isDeleted">Indicates if the user is marked as deleted.</param>
    /// <param name="deletionDate">The date of deletion, if applicable.</param>
    /// <param name="roles">Roles assigned to the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task SeedUserAsync(WebApplication app,
                                            string email,
                                            string password,
                                            bool twoFactorEnabled,
                                            bool isDeleted,
                                            DateTime? deletionDate = null,
                                            params string[] roles)
    {
        using var scope = app.Services.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var userMultiFactorStore = scope.ServiceProvider.GetRequiredService<IUserMultiFactorWriteStore>();

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            return;
        }

        User user = new()
        {
            UserName = email,
            Email = email,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = twoFactorEnabled,
            EmailConfirmed = true,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
            EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
            EntityDeletionStatus = new(isDeleted, deletionDate, isDeleted ? CreatedBy : null),
            Address = new Address(AddressValue, AddressValue, AddressValue, AddressValue, AddressValue)
        };

        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);

        await userManager.CreateAsync(user);

        foreach (var role in roles)
        {
            await userManager.AddToRoleAsync(user, role);
        }

        await userMultiFactorStore.CreateAsync(user.Id);
        
        if (twoFactorEnabled)
        {
            await userMultiFactorStore.SetEmailAsync(true, user.Id);
        }
    }

    /// <summary>
    /// Seeds roles into the database if they don't already exist.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <param name="roles">The roles to seed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task SeedRolesAsync(WebApplication app,
                                             params string[] roles)
    {
        using var scope = app.Services.CreateAsyncScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        // Create roles.
        foreach (var role in roles)
        {
            Role newRole = new()
            {
                Name = role,
                NormalizedName = role.ToUpper(CultureInfo.InvariantCulture),
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityDeletionStatus = new(false, null, null),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                ConcurrencyStamp = Ulid.NewUlid().ToString()
            };

            if (!await roleManager.RoleExistsAsync(newRole.Name))
            {
                await roleManager.CreateAsync(newRole);
            }
        }
    }
    /// <summary>
    /// Seeds all test user data.
    /// </summary>
    public static partial class Test
    {
        /// <summary>
        /// Deleted user for test data.
        /// </summary>
        public const string DeleteUserEmail = "deletedUser@default.com";
        /// <summary>
        /// User for test data.
        /// </summary>
        public const string AuthorizeUserEmail = "authorizeTest@default.com";
        /// <summary>
        /// User for mfa test data.
        /// </summary>
        public const string MultiFactorUserEmail = "multiFactorTest@default.com";
        /// <summary>
        /// User for account purge test data.
        /// </summary>
        public const string RecentlyDeletedUserEmail = "recentlydeleted@default.com";
        /// <summary>
        /// User for account purge test data.
        /// </summary>
        public const string OldDeletedUserEmail = "olddeleted@default.com";
    }
}