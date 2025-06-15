using Application.Stores;
using Domain.Contracts.Stores;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Persistence.Contexts;

namespace Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the persistence services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> </returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // Change from Singleton to Scoped lifetime
        services.AddDbContext<AppDbContext>(ServiceLifetime.Scoped);
        
        // Register stores
        services.TryAddScoped<IUserReadStore, UserReadStore>();
        services.TryAddScoped<IUserWriteStore, UserWriteStore>();
        services.TryAddScoped<IUserMultiFactorReadStore, UserMultiFactorReadStore>();
        services.TryAddScoped<IUserMultiFactorWriteStore, UserMultiFactorWriteStore>();
        services.TryAddScoped<IApplicationReadStore, ApplicationReadStore>();
        services.TryAddScoped<IApplicationWriteStore, ApplicationWriteStore>();
        services.TryAddScoped<ISharedStore, SharedStore>();
        services.TryAddScoped<IActivityReadStore, ActivityReadStore>();
        services.TryAddScoped<IActivityWriteStore, ActivityWriteStore>();
        services.TryAddScoped<ISessionReadStore, SessionReadStore>();
        services.TryAddScoped<ISessionWriteStore, SessionWriteStore>();
        services.TryAddScoped<IPasskeyCredentialReadStore, PasskeyCredentialReadStore>();
        services.TryAddScoped<IPasskeyCredentialWriteStore, PasskeyCredentialWriteStore>();
        
        return services;
    }
}