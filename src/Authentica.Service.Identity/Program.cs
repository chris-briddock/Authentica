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
using Api.Constants;
using ITimer = Application.Contracts.ITimer;
using Microsoft.AspNetCore.Identity;
using Domain.Aggregates.Identity;


namespace Authentica.Service.Identity;

/// <summary>
/// The entry point for the Web Application.
/// </summary>
public sealed class Program
{
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
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDataProtection();
        builder.Services.AddCustomControllers();
        builder.Services.AddMetrics();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddVersioning(1,0);
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSwaggerGen($"{ServiceNameDefaults.ServiceName}.xml");
        builder.Services.TryAddScoped<ISecretHasher, Argon2SecretHasher>();
        builder.Services.TryAddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
        builder.Services.TryAddScoped<IRandomStringProvider, RandomStringProvider>();
        builder.Services.TryAddTransient<ITimer, TimerProvider>();
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
        builder.Services.AddRedisHealthChecks(builder.Configuration["ConnectionStrings:Redis"]!);
        builder.Services.AddAzureApplicationInsightsHealthChecks();

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
        await app.UseDatabaseMigrationsAsync<AppDbContext>();
        await app.UseSeedDataAsync();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(CorsDefaults.PolicyName);
            await app.UseSeedTestDataAsync();
        }
        await app.RunAsync();
    }
}
