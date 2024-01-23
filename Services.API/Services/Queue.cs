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
        await queue.SendMessageAsync(MessageBody);
    }

     public async Task SendMessageAsync(ConfirmationRequest confirmationRequest)
    {
        QueueClient queue = new QueueClient(_configuration.GetConnectionString("StorageConnectionString"), _configuration.GetConnectionString("ConfirmationQueueName"));
        var MessageBody = JsonSerializer.Serialize(confirmationRequest);
        await queue.SendMessageAsync(MessageBody);
    }
}