using Serilog;
using Serilog.Sinks.AmazonSQS;

namespace Sample.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDefaultLogOptions(this IServiceCollection collection,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}{NewLine}")
            .WriteTo.AmazonSQS(
                configuration["AWS:AccessKey"],
                configuration["AWS:SecretKey"],
                configuration["AWS:ServiceUrl"],
                configuration["AWS:QueueUser"],
                configuration["AWS:QueueName"])
            .CreateLogger();

        collection.AddLogging(logginBuilder => { logginBuilder.AddSerilog(); });
        return collection;
    }
}