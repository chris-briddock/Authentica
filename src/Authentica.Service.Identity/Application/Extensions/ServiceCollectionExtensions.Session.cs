namespace Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds custom session configuration to the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCustomSession(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.Cookie.Name = ".AspNet.Session";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.IdleTimeout = TimeSpan.FromMinutes(60);
        });

        return services;
    }

}