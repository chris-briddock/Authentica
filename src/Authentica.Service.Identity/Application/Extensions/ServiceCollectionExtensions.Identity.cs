using Application.Factories;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Extensions;

public static partial class ServiceCollectionExtensions 
{
   
    /// <summary>
    /// Adds the ASP.NET Identity configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> </returns>
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(opt =>
        {
            opt.SignIn.RequireConfirmedPhoneNumber = false;
            opt.SignIn.RequireConfirmedEmail = false;
            opt.SignIn.RequireConfirmedAccount = false;
            opt.Lockout.AllowedForNewUsers = false;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            opt.Lockout.MaxFailedAccessAttempts = 5;
            opt.Password.RequireDigit = true;
            opt.Password.RequiredLength = 12;
            opt.Password.RequiredUniqueChars = 1;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequireUppercase = true;
            opt.User.RequireUniqueEmail = false;
            opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddErrorDescriber<IdentityErrorFactory>()
        .AddSignInManager<SignInManager<User>>()
        .AddUserManager<UserManager<User>>()
        .AddRoleManager<RoleManager<Role>>()
        .AddRoles<Role>()
        .AddRoleStore<RoleStore<Role, AppDbContext, string, UserRole, RoleClaim>>()
        .AddUserStore<UserStore<User, Role, AppDbContext, string, UserClaim, UserRole, IdentityUserLogin<string>, IdentityUserToken<string>, RoleClaim>>()
        .AddDefaultTokenProviders();

        return services;
    }
}