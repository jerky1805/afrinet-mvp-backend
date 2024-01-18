using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Activation.Processor.Models;
using Afrinet.Models;
using System.Text.Json;
using System.Security.Cryptography;


namespace Activation.Processor;



public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IConfiguration _configuration;

    private readonly ActivationService _activationService;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, ActivationService activationService)
    {
        _logger = logger;
        _configuration = configuration;
        _activationService = activationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            QueueClient queue = new QueueClient(_configuration.GetConnectionString("StorageConnectionString"), _configuration.GetConnectionString("QueueName"));

            if (await queue.ExistsAsync())
            {
                QueueProperties properties = await queue.GetPropertiesAsync();

                if (properties.ApproximateMessagesCount > 0)
                {
                    QueueMessage[] retrievedMessage = await queue.ReceiveMessagesAsync(1);
                    string theMessage = retrievedMessage[0].Body.ToString();
                    ActivationRequest ar = JsonSerializer.Deserialize<ActivationRequest>(theMessage);
                    //TODO: Send TOTP SMS to Supplied MSISDN
                    ar.TOTP = RandomNumberGenerator.GetInt32(1, 1000000).ToString();
                    await _activationService.CreateActivation(ar);
                    await queue.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    Console.WriteLine("Message: " + theMessage);
                }

            }
            await Task.Delay(20, stoppingToken);
        }
    }
}
