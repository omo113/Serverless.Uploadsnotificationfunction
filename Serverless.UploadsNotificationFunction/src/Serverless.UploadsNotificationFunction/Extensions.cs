using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Serverless.UploadsNotificationFunction;

public static class Extensions
{
    public static Bootstrapper BuildConfiguration(this Bootstrapper bootstrapper)
    {
        bootstrapper.Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();
        return bootstrapper;
    }

    public static Bootstrapper Use<TStartup>(this Bootstrapper bootstrapper) where TStartup : IStartup, new()
    {
        bootstrapper.Startup = new TStartup();
        bootstrapper.Startup.ConfigureServices(bootstrapper.Configuration);
        return bootstrapper;
    }

    public static IServiceProvider Run(this Bootstrapper bootstrapper)
    {
        return bootstrapper.Startup.Services.BuildServiceProvider();
    }
}