using Application.Contracts;
using Application.Stores;
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
        services.AddDbContext<AppDbContext>(ServiceLifetime.Singleton);

        services.TryAddScoped<IApplicationReadStore, ApplicationReadStore>();
        services.TryAddScoped<IApplicationWriteStore, ApplicationWriteStore>();
        services.TryAddScoped<IUserReadStore, UserReadStore>();
        services.TryAddScoped<IUserWriteStore, UserWriteStore>();
        services.TryAddScoped<IActivityReadStore, ActivityReadStore>();
        services.TryAddScoped<IActivityWriteStore, ActivityWriteStore>();
        services.TryAddScoped<ISharedStore, SharedStore>();

        return services;
    }
}