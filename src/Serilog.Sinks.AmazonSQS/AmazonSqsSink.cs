using Amazon.SQS.Model;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Serilog.Sinks.AmazonSQS;

public class AmazonSqsSink : ILogEventSink
{
    private readonly string _awsAccessKeyId;
    private readonly string _awsSecretAccessKey;
    private readonly string _queueUrl;
    private readonly string _serviceUrl;
    private readonly string _template;
    private readonly MessageTemplateTextFormatter _textFormatter;

    public AmazonSqsSink(string awsAccessKeyId, string awsSecretAccessKey, string serviceUrl, string queueUser,
        string queueName, string template)
    {
        _awsAccessKeyId = awsAccessKeyId;
        _awsSecretAccessKey = awsSecretAccessKey;
        _serviceUrl = serviceUrl;
        _template = template;
        _queueUrl = $"{_serviceUrl}/{queueUser}/{queueName}";
        _textFormatter = new MessageTemplateTextFormatter(template);
    }

    public void Emit(LogEvent logEvent)
    {
        var sqsClient = SqsClientFactory.CreateClient(_awsAccessKeyId, _awsSecretAccessKey, _serviceUrl);

        var sw = new StringWriter();
        _textFormatter.Format(logEvent, sw);

        var sendRequest = new SendMessageRequest
        {
            MessageBody = sw.ToString(),
            QueueUrl = _queueUrl
        };

        sqsClient.SendMessageAsync(sendRequest).SyncContextSafeWait();
    }
}