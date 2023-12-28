using System.Text.Json;
using System.Text;
using Afrinet.API.Contracts;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Services.API.Models;
using Afrinet.Models;

namespace Services.API.Services;

public class Queue : IQueue
{
    private readonly IConfiguration _configuration;
    public Queue(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    
    public async Task SendMessageAsync(ActivationRequest activationRequest)
    {
        QueueClient queue = new QueueClient(_configuration.GetConnectionString("StorageConnectionString"), _configuration.GetConnectionString("ActivationQueueName"));
        var MessageBody = JsonSerializer.Serialize(activationRequest);
        // var message = new Message(Encoding.UTF8.GetBytes(MessageBody))
        // {
        //     ContentType = "application/json",
        //     MessageId = Guid.NewGuid().ToString(),

        // };
        await queue.SendMessageAsync(MessageBody);
    }
}