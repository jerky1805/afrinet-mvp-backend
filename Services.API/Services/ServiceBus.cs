using System.Text.Json;
using System.Text;
using Afrinet.API.Contracts;
using Microsoft.Azure.ServiceBus;
using Services.API.Models;

namespace Services.API.Services;

public class ServiceBus: IServiceBus{
    private readonly IConfiguration _configuration;
    public ServiceBus(IConfiguration configuration){
        _configuration = configuration;
    }
    public async Task SendMessageAsync(OrderDetails orderDetails){
        IQueueClient client = new QueueClient(_configuration["ServiceBusConnectionString"], _configuration["OrderQueueName"]);
        var MessageBody = JsonSerializer.Serialize(orderDetails);
        var message = new Message(Encoding.UTF8.GetBytes(MessageBody)){
            ContentType = "application/json",
            MessageId = Guid.NewGuid().ToString(),
            
        };
        await client.SendAsync(message);
    }
}