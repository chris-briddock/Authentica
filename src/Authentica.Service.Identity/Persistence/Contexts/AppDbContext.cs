using System.Reflection;
using ChristopherBriddock.AspNetCore.Extensions;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

/// <summary>
/// Main database context for the application.
/// </summary>
public sealed class AppDbContext : DbContext 
{
    /// <summary>
    /// Configuration instance for the DbContext.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="opt">DbContext options.</param>
    /// <param name="configuration">Configuration instance.</param>
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
        });
    }

    /// <summary>
    /// Applies entity configurations.
    /// </summary>
    /// <param name="modelBuilder">Model builder instance.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Users collection.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Roles collection.
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// UserRoles collection.
    /// </summary>
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    /// <summary>
    /// ClientApplications collection.
    /// </summary>
    public DbSet<ClientApplication> ClientApplications => Set<ClientApplication>();

    /// <summary>
    /// UserClientApplications collection.
    /// </summary>
    public DbSet<UserClientApplication> UserClientApplications => Set<UserClientApplication>();

    /// <summary>
    /// UserClaims collection.
    /// </summary>
    public DbSet<UserClaim> UserClaims => Set<UserClaim>();

    /// <summary>
    /// RoleClaims collection.
    /// </summary>
    public DbSet<RoleClaim> RoleClaims => Set<RoleClaim>();

    /// <summary>
    /// UserTokens collection.
    /// </summary>
    public DbSet<IdentityUserToken<string>> UserTokens => Set<IdentityUserToken<string>>();

    /// <summary>
    /// UserLogins collection.
    /// </summary>
    public DbSet<IdentityUserLogin<string>> UserLogins => Set<IdentityUserLogin<string>>();

    /// <summary>
    /// Activities collection.
    /// </summary>
    public DbSet<Activity> Activities => Set<Activity>();

    /// <summary>
    /// Sessions collection.
    /// </summary>
    public DbSet<Session> Sessions => Set<Session>();
}
