using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Serverless.UploadsNotificationFunction;

public interface IStartup
{
    IServiceCollection Services { get; }
    void ConfigureServices(IConfiguration configuration);
}