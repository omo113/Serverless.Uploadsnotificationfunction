namespace Serverless.UploadsNotificationFunction.Configurations;

public class SnsConfiguration
{
    public const string Section = "SnsConfiguration";
    public required string TopicArn { get; set; }
}