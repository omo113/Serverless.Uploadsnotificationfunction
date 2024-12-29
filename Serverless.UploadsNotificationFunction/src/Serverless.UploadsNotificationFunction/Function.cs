using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serverless.UploadsNotificationFunction.Configurations;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Serverless.UploadsNotificationFunction;

public sealed class Function
{
    private IAmazonSimpleNotificationService _snsClient;
    private SnsConfiguration _snsConfiguration;

    // Update this to your SNS Topic ARN
    private string TopicArn => _snsConfiguration.TopicArn;

    /// <summary>
    /// Default constructor. The AWS credentials will come from the IAM role associated with this function,
    /// and the AWS region will be set to the region in which this Lambda function is executed.
    /// </summary>
    public Function()
    {
        Bootup();
    }

    /// <summary>
    /// This method is called for every Lambda invocation. 
    /// It takes in an SQS event object and processes each record by sending it to SNS.
    /// </summary>
    /// <param name="sqsEvent">The SQS event containing the batch of messages.</param>
    /// <param name="context">The Lambda context which provides logging and function details.</param>
    /// <returns>An object indicating the result of the function execution.</returns>
    public async Task<object> FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {


        if (sqsEvent?.Records == null || sqsEvent.Records.Count == 0)
        {
            return new
            {
                statusCode = 200,
                body = "No messages to process. Lambda function completed"
            };
        }
        context.Logger.LogInformation(TopicArn);

        var processedCount = await ProcessRecords(sqsEvent.Records, context);

        context.Logger.LogInformation($@"
                SNS TOPIC ARN         = {TopicArn}
                Function Name         = {context.FunctionName}
                Processed Messages    = {processedCount}
                Remaining Time (ms)   = {context.RemainingTime}
            ");

        return new
        {
            statusCode = 200,
            body = "Lambda function completed"
        };
    }

    /// <summary>
    /// Processes each SQS record, publishing the record body to an SNS topic.
    /// </summary>
    /// <param name="records">A collection of SQS messages.</param>
    /// <param name="context">Lambda execution context for logging.</param>
    /// <returns>The count of processed records.</returns>
    private async Task<int> ProcessRecords(IEnumerable<SQSEvent.SQSMessage> records, ILambdaContext context)
    {
        int count = 0;

        foreach (var record in records)
        {
            if (string.IsNullOrEmpty(record.Body))
            {
                throw new InvalidOperationException("No body in SQS record.");
            }

            // Publish to SNS
            await _snsClient.PublishAsync(new PublishRequest
            {
                TopicArn = TopicArn ?? "arn:aws:sns:eu-central-1:207567801889:omari-sns",
                Subject = "Processed SQS Queue Messages",
                Message = record.Body
            });

            count++;
        }

        return count;
    }

    private void Bootup()
    {
        try
        {
            var services = Bootstrapper.CreateBootstrapper()
                .BuildConfiguration()
                .Use<Startup>()
                .Run();
            _snsClient = services.GetRequiredService<IAmazonSimpleNotificationService>();
            _snsConfiguration = services.GetRequiredService<IOptions<SnsConfiguration>>().Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FAILURE: Function Bootup Failure: {ex}");
        }
    }
}