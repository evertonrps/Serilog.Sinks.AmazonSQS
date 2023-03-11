using Amazon.SQS;

namespace Serilog.Sinks.AmazonSQS;

public class SqsClientFactory
{
    public static AmazonSQSClient CreateClient(string accessKeyId, string secretAccessKey, string serviceUrl)
    {
        return new AmazonSQSClient(
            accessKeyId,
            secretAccessKey,
            new AmazonSQSConfig { ServiceURL = serviceUrl });
    }
}