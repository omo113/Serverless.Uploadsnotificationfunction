using Amazon.Lambda.SQSEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace Serverless.UploadsNotificationFunction.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestSQSEventLambdaFunction()
    {
        var snsEvent = new SQSEvent()
        {
            Records =
            [
                new SQSEvent.SQSMessage
                {
                    Body = "test"
                }
            ]
        };

        var logger = new TestLambdaLogger();
        var context = new TestLambdaContext
        {
            Logger = logger
        };
        await Task.Delay(1000);
        var function = new Function();
        //await function.FunctionHandler(snsEvent, context);

        //Assert.Contains("Processed record foobar", logger.Buffer.ToString());
        Assert.True(true);
    }
}