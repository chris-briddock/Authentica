using Api.Middlware;
using Application.BackgroundServices;
using Application.Cryptography;
using Application.Extensions;
using Application.Providers;
using ChristopherBriddock.AspNetCore.Extensions;
using ChristopherBriddock.AspNetCore.HealthChecks;
using Common.Extensions;
using Domain.Aggregates.Identity;
using Domain.Constants;
using Domain.Contracts.Cryptography;
using Domain.Contracts.Providers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using Persistence.Contexts;
using System.IdentityModel.Tokens.Jwt;
using HostApplicationBuilderExtensions = Application.Extensions.HostApplicationBuilderExtensions;


namespace Authentica.Service.Identity;

/// <summary>
/// The entry point for the Web Application.
/// </summary>
public sealed class Program
{
    private Program(){}
    /// <summary>
    /// The entry method for the web application.
    /// </summary>
    public static async Task Main()
    {
        AppDomain.CurrentDomain.SetData("LOADER_OPTIMIZATION", "SingleDomain");
        DotNetEnv.Env.Load();
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        HostApplicationBuilderExtensions.ConfigureOpenTelemetry(builder, ServiceNameDefaults.ServiceName);
        builder.WebHost.AddKestrelConfiguration(7171);
        builder.Services.Configure<HostOptions>(x =>
        {
            x.ServicesStartConcurrently = true;
            x.ServicesStopConcurrently = true;
        });
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.Configure<DataProtectionTokenProviderOptions>(x => x.TokenLifespan = TimeSpan.FromMinutes(5));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddFeatureManagement();
        builder.Services.AddDataProtection();
        builder.Services.AddControllers();
        builder.Services.AddMetrics();
        builder.Services.AddResponseCaching();
        builder.Services.AddResponseCompression(opt => opt.EnableForHttps = true);
        builder.Services.AddOutputCache(opt => opt.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(5));
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddVersioning(2, 0);
        builder.Services.AddDistributedCache(builder.Configuration);
        builder.Services.AddInMemoryCache();
        builder.Services.AddAzureAppInsights();
        builder.Services.AddHybridCache(builder.Configuration);
        builder.Services.AddSwaggerGen($"{ServiceNameDefaults.ServiceName}.xml");
        builder.Services.AddPersistence();
        builder.Services.AddPasskeys(builder.Configuration);
        builder.Services.TryAddScoped<IPasskeyTokenProvider<User>, PasskeyTokenProvider<User>>();
        builder.Services.TryAddScoped<ISecretHasher, Argon2SecretHasher>();
        builder.Services.TryAddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
        builder.Services.TryAddScoped<IRandomStringProvider, RandomStringProvider>();
        builder.Services.TryAddTransient<ITimerProvider, TimerProvider>();
        builder.Services.TryAddScoped<JwtSecurityTokenHandler>();
        builder.Services.TryAddScoped<IScopeProvider, ScopeProvider>();
        builder.Services.TryAddScoped<IMultiFactorTotpProvider, MultiFactorTotpProvider>();
        builder.Services.AddBearerAuthentication(builder.Configuration);
        builder.Services.AddCrossOrigin();
        builder.Services.AddCustomSession();
        builder.Services.AddIdentity();
        builder.Services.AddPublisherMessaging(builder.Configuration);
        builder.Services.AddHostedService<AccountPurge>();
        builder.Services.AddHostedService<ApplicationPurge>();
        builder.Services.AddSqlDatabaseHealthChecks(builder.Configuration.GetConnectionString("DefaultConnection")!);
        builder.Services.AddRedisHealthCheck(builder.Configuration);


        WebApplication app = builder.Build();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<SessionMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseHsts();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseCustomHealthCheckMapping();
        await app.UseDatabaseMigrationsAsync<AppDbContext>();
        await app.UseSeedDataAsync();
        if (app.Environment.IsDevelopment())
        {
            app.UseCors(CorsDefaults.PolicyName);
            app.UseSwagger();
            app.UseSwaggerUI();
            await app.UseSeedTestDataAsync();
        }
        app.UseResponseCaching();
        app.UseOutputCache();
        await app.RunAsync();
    }
}
