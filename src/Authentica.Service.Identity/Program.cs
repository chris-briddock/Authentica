using Api.Middlware;
using Application.BackgroundServices;
using Application.Cryptography;
using Application.Extensions;
using Application.Providers;
using Authentica.Common;
using ChristopherBriddock.AspNetCore.Extensions;
using ChristopherBriddock.AspNetCore.HealthChecks;
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


namespace Authentica.Service.Identity;

/// <summary>
/// The entry point for the Web Application.
/// </summary>
public sealed class Program
{
    /// <summary>
    /// The entry method for the web application.
    /// </summary>
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateSlimBuilder();
        builder.ConfigureOpenTelemetry(ServiceNameDefaults.ServiceName);
        builder.Services.Configure<HostOptions>(x =>
        {
            x.ServicesStartConcurrently = true;
            x.ServicesStopConcurrently = true;
        });
        builder.Services.Configure<DataProtectionTokenProviderOptions>(x => x.TokenLifespan = TimeSpan.FromMinutes(5));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddFeatureManagement();
        builder.Services.AddDataProtection();
        builder.Services.AddControllers();
        builder.Services.AddMetrics();
        builder.Services.AddResponseCaching();
        builder.Services.AddResponseCompression(opt => opt.EnableForHttps = true);
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddVersioning(2, 0);
        builder.Services.AddDistributedCache(builder.Configuration);
        builder.Services.AddInMemoryCache();
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
        builder.Services.AddAzureAppInsights();
        builder.Services.AddCrossOrigin();
        builder.Services.AddCustomSession();
        builder.Services.AddIdentity();
        builder.Services.AddPublisherMessaging(builder.Configuration);
        builder.Services.AddHostedService<AccountPurge>();
        builder.Services.AddHostedService<ApplicationPurge>();
        builder.Services.AddSqlDatabaseHealthChecks(builder.Configuration["ConnectionStrings:DefaultConnection"]!);
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
        app.UseResponseCaching();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseCustomHealthCheckMapping();
        if (!app.Environment.IsProduction())
        {
            await app.UseDatabaseMigrationsAsync<AppDbContext>();
            app.UseCors(CorsDefaults.PolicyName);
        }
        await app.UseSeedDataAsync();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            await app.UseSeedTestDataAsync();
        }
        await app.RunAsync(); 
    }
}
