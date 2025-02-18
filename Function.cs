using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.S3;
using Amazon.S3.Model;
using System.Text.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaSQSConsumer;

public class Function
{
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>

    private readonly IAmazonS3 _s3Client;
    private const string BUCKET_NAME = "eks-processing";

    public Function()
    {
        _s3Client = new AmazonS3Client();
    }


    public Function(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }



    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        context.Logger.LogInformation("The SQS Event" + JsonSerializer.Serialize(evnt));
        foreach(var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        context.Logger.LogInformation("Beginning to process message with MessageId: " + message.MessageId);
        var objectRequest = new PutObjectRequest
        {
            BucketName = BUCKET_NAME,
            Key = $"processed-eks-{message.MessageId}.json",
            ContentBody = message.Body,
            ContentType = "application/json",
        };

        var response = await _s3Client.PutObjectAsync(objectRequest);

        context.Logger.LogInformation($"Processed message: {message.Body}");
        await Task.CompletedTask;
    }
}