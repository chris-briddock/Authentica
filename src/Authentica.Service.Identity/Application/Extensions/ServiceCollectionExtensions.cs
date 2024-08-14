using System.Text;
using Application.Contracts;
using Application.Factories;
using Application.Providers;
using Application.Publishers;
using Application.Stores;
using Authentica.Common;
using ChristopherBriddock.AspNetCore.Extensions;
using Domain.Aggregates.Identity;
using Domain.Constants;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Contexts;


namespace Application.Extensions;

/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions 
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
        services.TryAddScoped<IEventStore, EventStore>();
        services.TryAddScoped<ISharedStore, SharedStore>();

        return services;
    }

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

    /// <summary>
    /// Add the required services for in-memory and redis services, if redis is enabled in the feature flags.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddSessionCache(this IServiceCollection services)
    {
        IConfiguration configuration = services
                                      .BuildServiceProvider()
                                      .GetService<IConfiguration>()!;
        IFeatureManager featureManager = services
                                        .BuildServiceProvider()
                                        .GetService<IFeatureManager>()!;

        services.AddDistributedMemoryCache();

        if (!featureManager.IsEnabledAsync(FeatureFlagConstants.Cache).Result)
            return services;
        
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration.GetConnectionString("Redis");
        });
        
        return services;
    }


    /// <summary>
    /// Adds bearer authentication services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddBearerAuthentication(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.TryAddScoped<IJsonWebTokenProvider, JsonWebTokenProvider>();
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => 
        {
            // Retrieve the JWT secret from the application configuration
            string issuer = configuration.GetRequiredValueOrThrow("Jwt:Issuer");
            string audience = configuration.GetRequiredValueOrThrow("Jwt:Audience");
            string jwtSecret = configuration.GetRequiredValueOrThrow("Jwt:Secret");

            var key = Encoding.ASCII.GetBytes(jwtSecret);
            // Configure token validation parameters
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            // Save the token in the authentication context
            options.SaveToken = true;
        });

        return services;
    }

    /// <summary>
    /// Adds publisher messaging for rabbitmq or azure service bus.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddPublisherMessaging(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider()
                                    .GetService<IConfiguration>()!;

        var featureManager = services.BuildServiceProvider()
                                     .GetService<IFeatureManager>()!;

        var rabbitMqEnabled = featureManager.IsEnabledAsync(FeatureFlagConstants.RabbitMq).Result;

        var azServiceBusEnabled = featureManager.IsEnabledAsync(FeatureFlagConstants.AzServiceBus).Result;

        if (azServiceBusEnabled)
        {
            services.AddMassTransit(mt =>
            {
                mt.UsingAzureServiceBus((context, config) =>
                {
                    config.Host(configuration["ConnectionStrings:AzureServiceBus"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
        if (rabbitMqEnabled)
        {
            services.AddMassTransit(mt =>
            {
                mt.SetKebabCaseEndpointNameFormatter();

                mt.UsingRabbitMq((context, config) =>
                {

                    config.Host(configuration["RabbitMQ:Hostname"], "/", r =>
                    {
                        r.Username(configuration["RabbitMQ:Username"]!);
                        r.Password(configuration["RabbitMQ:Password"]!);
                    });
                    config.ConfigureEndpoints(context);
                });
            });
        }

        if (rabbitMqEnabled || azServiceBusEnabled)
        {
            services.TryAddTransient<IEmailPublisher, EmailPublisher>();
        }
        else
        {
            services.TryAddTransient<IEmailPublisher, NullEmailPublisher>();
        }

        return services;
    }
}