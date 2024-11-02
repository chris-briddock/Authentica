using ChristopherBriddock.AspNetCore.Extensions;
using Domain.Constants;

namespace Application.Extensions;

/// <summary>
/// Provides extension methods for setting up passkey authentication services in the dependency injection container.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds passkey authentication services to the <see cref="IServiceCollection"/> with configuration values for origin and domain.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the passkey services will be added.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance with the passkey services configured.</returns>
    /// <exception cref="InvalidOperationException">Thrown if required configuration values are missing.</exception>
    public static IServiceCollection AddPasskeys(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var origin = configuration.GetRequiredValueOrThrow("Passkeys:Origin");

        services.AddFido2(x =>
        {
            x.ServerName = ServiceNameDefaults.ServiceName;
            x.Origins = [origin];
            x.ServerDomain = configuration.GetRequiredValueOrThrow("Passkeys:Domain");
            x.TimestampDriftTolerance = 100000;
        });

        return services;
    }
}
