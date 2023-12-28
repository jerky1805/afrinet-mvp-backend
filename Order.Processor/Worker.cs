using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace Order.Processor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
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
                    await queue.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    Console.WriteLine("Message: " + theMessage);
                }

            }
            await Task.Delay(20, stoppingToken);
        }
    }
}
