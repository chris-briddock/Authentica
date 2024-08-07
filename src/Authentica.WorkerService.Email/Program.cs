using System.Net.Mail;
using ChristopherBriddock.AspNetCore.Extensions;
using Microsoft.FeatureManagement;

namespace Authentica.WorkerService.Email;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.ConfigureOpenTelemetry(ServiceNameDefaults.ServiceName);
        builder.Services.AddFeatureManagement();
        builder.Services.AddConsumerMessaging();
        builder.Services.AddScoped<ISmtpClient, SmtpClientWrapper>();
        builder.Services.AddScoped<SmtpClient>();

        var host = builder.Build();
        await host.RunAsync();

    }
}