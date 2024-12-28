using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serverless.UploadsNotificationFunction.Configurations;

namespace Serverless.UploadsNotificationFunction;

public class Startup : IStartup
{
    public IServiceCollection Services { get; set; } = null!;

    public void ConfigureServices(IConfiguration configuration)
    {
        Services = new ServiceCollection()
            .AddLogging(x =>
            {
                x.SetMinimumLevel(LogLevel.Trace);
                x.ClearProviders();
                x.AddConsole();
            });
        Services.AddSingleton<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
        Services.Configure<SnsConfiguration>(configuration.GetSection(nameof(SnsConfiguration.Section)));
    }
}