using System.Reflection;
using ChristopherBriddock.AspNetCore.Extensions;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

/// <summary>
/// Represents the main DbContext for the application.
/// </summary>
public sealed class AppDbContext : DbContext 
{
    /// <summary>
    /// Gets the configuration instance injected into the DbContext.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="opt">The options for configuring the DbContext.</param>
    /// <param name="configuration">The configuration instance injected into the DbContext.</param>
    public AppDbContext(DbContextOptions<AppDbContext> opt,
                        IConfiguration configuration) : base(opt)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Configures the DbContext options such as database connection string and retry policy.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure DbContext options.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {  
        optionsBuilder.UseSqlServer(Configuration.GetConnectionStringOrThrow("Default"), opt => 
        {
            opt.EnableRetryOnFailure();
        })
        .EnableDetailedErrors();
    }

    /// <summary>
    /// Applies entity configurations defined in the current assembly to the DbContext.
    /// </summary>
    /// <param name="builder">The model builder instance used to apply entity configurations.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    /// <summary>
    /// Represents a collection of Users within the DbContext.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Represents a collection of Roles within the DbContext.
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// Represents a collection of UserRoles within the DbContext.
    /// </summary>
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    /// <summary>
    /// Represents a collection of ClientApplications within the DbContext.
    /// </summary>
    public DbSet<ClientApplication> ClientApplications => Set<ClientApplication>();

    /// <summary>
    /// Represents a collection of UserClientApplications within the DbContext.
    /// </summary>
    public DbSet<UserClientApplication> UserClientApplications => Set<UserClientApplication>();

    /// <summary>
    /// Represents a collection of UserClaims within the DbContext.
    /// </summary>
    public DbSet<UserClaim> UserClaims => Set<UserClaim>();

    /// <summary>
    /// Represents a collection of RoleClaims within the DbContext.
    /// </summary>
    public DbSet<RoleClaim> RoleClaims => Set<RoleClaim>();
    /// <summary>
    /// Represents a collection of UserTokens within the DbContext.
    /// </summary>
    public DbSet<IdentityUserToken<string>> UserTokens => Set<IdentityUserToken<string>>();
    /// <summary>
    /// Represents a collection of UserLogins within the DbContext.
    /// </summary>
    public DbSet<IdentityUserLogin<string>> UserLogins => Set<IdentityUserLogin<string>>();
    /// <summary>
    /// Represents a collection of Events within the DbContext.
    /// </summary>
    public DbSet<Event> Events => Set<Event>();
}
