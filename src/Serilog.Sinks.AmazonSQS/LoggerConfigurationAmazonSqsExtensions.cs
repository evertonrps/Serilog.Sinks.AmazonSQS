using Serilog.Configuration;

namespace Serilog.Sinks.AmazonSQS;

public static class LoggerConfigurationAmazonSqsExtensions
{
    private const string DefaultOutputTemplate =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}{NewLine}";

    public static LoggerConfiguration AmazonSQS(
        this LoggerSinkConfiguration sinkConfiguration,
        string awsAccessKeyId,
        string awsSecretAccessKey,
        string serviceUrl,
        string queueUser,
        string queueName,
        string? outputTemplate = DefaultOutputTemplate
    )
    {
        if (sinkConfiguration is null) throw new ArgumentNullException(nameof(sinkConfiguration));

        if (string.IsNullOrWhiteSpace(awsAccessKeyId)) throw new ArgumentNullException(nameof(awsAccessKeyId));

        if (string.IsNullOrWhiteSpace(awsSecretAccessKey)) throw new ArgumentNullException(nameof(awsSecretAccessKey));

        if (string.IsNullOrWhiteSpace(serviceUrl)) throw new ArgumentNullException(nameof(serviceUrl));

        if (string.IsNullOrWhiteSpace(queueUser)) throw new ArgumentNullException(nameof(queueUser));

        if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentNullException(nameof(queueName));

        if (string.IsNullOrWhiteSpace(outputTemplate)) outputTemplate = DefaultOutputTemplate;

        var sink = new AmazonSqsSink(awsAccessKeyId, awsSecretAccessKey, serviceUrl, queueUser, queueName,
            outputTemplate);
        return sinkConfiguration.Sink(sink);
    }
}