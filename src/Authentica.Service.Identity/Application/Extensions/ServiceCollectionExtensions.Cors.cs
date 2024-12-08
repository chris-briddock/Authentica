using Domain.Constants;

namespace Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds cross-origin policies for the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCrossOrigin(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy(CorsDefaults.PolicyName, opt =>
            {
                opt.WithOrigins("http://localhost:3000");
                opt.AllowAnyHeader();
                opt.AllowAnyMethod();
                opt.AllowCredentials();
            });
        });

        return services;
    }
}