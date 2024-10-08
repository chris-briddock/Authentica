using Api.Middlware;
using Application.BackgroundServices;
using Application.Contracts;
using Application.Cryptography;
using Application.Extensions;
using Application.Providers;
using ChristopherBriddock.AspNetCore.Extensions;
using ChristopherBriddock.AspNetCore.HealthChecks;
using Domain.Constants;
using Microsoft.FeatureManagement;
using Persistence.Contexts;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ITimer = Application.Contracts.ITimer;
using Microsoft.AspNetCore.Identity;
using Domain.Aggregates.Identity;
using Authentica.Common;
using Application.Stores;


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
    /// <param name="args"></param>
    /// <returns></returns>
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.ConfigureOpenTelemetry(ServiceNameDefaults.ServiceName);
        builder.Services.Configure<HostOptions>(options =>
        {
            options.ServicesStartConcurrently = true;
            options.ServicesStopConcurrently = true;
        });
        builder.Services.Configure<DataProtectionTokenProviderOptions>(x => x.TokenLifespan = TimeSpan.FromMinutes(5));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDataProtection();
        builder.Services.AddControllers();
        builder.Services.AddMetrics();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddVersioning(1,0);
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSwaggerGen($"{ServiceNameDefaults.ServiceName}.xml");
        builder.Services.TryAddScoped<ISecretHasher, Argon2SecretHasher>();
        builder.Services.TryAddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
        builder.Services.TryAddScoped<IRandomStringProvider, RandomStringProvider>();
        builder.Services.TryAddScoped<ISessionWriteStore, SessionWriteStore>();
        builder.Services.TryAddScoped<ISessionReadStore, SessionReadStore>();
        builder.Services.TryAddTransient<ITimer, TimerProvider>();
        builder.Services.TryAddScoped<IScopeProvider, ScopeProvider>();
        builder.Services.TryAddScoped<IMultiFactorTotpProvider, MultiFactorTotpProvider>();
        builder.Services.AddFeatureManagement();
        builder.Services.AddBearerAuthentication();
        builder.Services.AddSessionCache();
        builder.Services.AddAzureAppInsights();
        builder.Services.AddCrossOrigin();
        builder.Services.AddCustomSession();
        builder.Services.AddIdentity();
        builder.Services.AddPersistence();
        builder.Services.AddPublisherMessaging();
        builder.Services.AddHostedService<AccountPurge>();
        builder.Services.AddHostedService<ApplicationPurge>();
        builder.Services.AddSqlDatabaseHealthChecks(builder.Configuration.GetConnectionStringOrThrow("Default"));
        builder.Services.AddRedisHealthCheck();

        WebApplication app = builder.Build();
        app.UseSession();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<SessionMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseHsts();
        app.UseResponseCaching();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
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
