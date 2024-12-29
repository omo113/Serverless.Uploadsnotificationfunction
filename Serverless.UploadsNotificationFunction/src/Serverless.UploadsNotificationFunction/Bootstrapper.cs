using Microsoft.Extensions.Configuration;

namespace Serverless.UploadsNotificationFunction;

public class Bootstrapper
{
    public IConfigurationRoot Configuration;

    public IStartup Startup;

    public static Bootstrapper CreateBootstrapper()
    {
        return new Bootstrapper();
    }
}