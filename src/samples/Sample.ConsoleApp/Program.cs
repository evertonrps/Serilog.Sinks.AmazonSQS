// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.AmazonSQS;

Console.WriteLine("Hello, World!");

using var host = Host.CreateDefaultBuilder(args).Build();

var configuration = host.Services.GetRequiredService<IConfiguration>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.AmazonSQS(
        configuration["AWS:AccessKey"],
        configuration["AWS:SecretKey"],
        configuration["AWS:ServiceUrl"],
        configuration["AWS:QueueUser"],
        configuration["AWS:QueueName"])
    .CreateLogger();
Log.Information("Console OK! it's working");