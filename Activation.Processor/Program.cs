using Activation.Processor;
using Activation.Processor.Models;
using Microsoft.Extensions.Configuration;

// builder.Services.Configure<ActivationDatabaseSettings>(
// builder.Configuration.GetSection("ActivationDatabase"));

//builder.Services.AddSingleton<ActivationService>();


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.Configure<ActivationDatabaseSettings>(configuration.GetSection("ActivationDatabase"));
        services.AddSingleton<ActivationService>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
