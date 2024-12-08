using Microsoft.OpenApi.Models;

namespace Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Swagger with custom configuration.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="xmlFile"></param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddSwaggerGen(this IServiceCollection services, string xmlFile)
    {
        string AuthSchemeName = "Bearer";
        services.AddSwaggerGen(opt =>
        {
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            opt.IncludeXmlComments(xmlPath);
            opt.AddSecurityDefinition(AuthSchemeName, new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = AuthSchemeName
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = AuthSchemeName
                            },
                            Scheme = "oauth2",
                            Name = AuthSchemeName,
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
            });
            opt.UseApiEndpoints();
        });

        return services;
    }
}