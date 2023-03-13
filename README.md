# Serilog.Sinks.AmazonSQS

Serilog.Sinks.AmazonSQS is a library to send logging information from Serilog to Amazon SQS. This implementation was inspired by [Serilog.Sinks.AmazonS3](https://github.com/serilog-contrib/Serilog.Sinks.AmazonS3) and [Serilog.Sinks.AzureQueueStoragev2](https://github.com/GMIRelayMed/Serilog.Sinks.AzureQueueStoragev2) projects

## Getting Started

### Simple Example
```C#
Log.Logger = new LoggerConfiguration()
    .WriteTo.AmazonSQS(
        "YourAwsAccessKey",
        "YourAwsSecretKey",
        "YourAwsServiceUr}", //example: https://sqs.sa-east-1.amazonaws.com
        "YourAwsQueueUser",
        "YourAwsQueueName")
    .CreateLogger();
    
Log.Information("Hello, world!");   
 
```

### Complex Example

### JSON `appsettings.json` configuration
```JSON
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AWS": {
    "Profile": "default",
    "Region": "sa-east-1",
    "AccessKey": "KEY",
    "SecretKey": "KEY",
    "ServiceUrl": "URL",
    "QueueUser": "USER",
    "QueueName": "QUEUE"
  }
}

```
### C# `ServiceCollectionExtension.cs` configuration
```C#
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDefaultLogOptions(this IServiceCollection collection,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()          
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
```

### C# `Program.cs` configuration
```C#

    /* 
     <ItemGroup>
        <PackageReference Include="Serilog" Version="2.12.0"/>
        <PackageReference Include="Serilog.AspNetCore" Version="6.1.0"/>        
    </ItemGroup>     
     */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Serilog

builder.Services.AddDefaultLogOptions(builder.Configuration);
builder.Host.UseSerilog(Log.Logger);

#endregion


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

```

### C# `HomeController.cs` configuration

```C#
[ApiController]
[Route("[controller]")]
public class HomeController: ControllerBase
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet("test")]
    public IActionResult Get()
    {
        _logger.LogInformation("Hello, world!");
        return Ok();
    }
}
```
