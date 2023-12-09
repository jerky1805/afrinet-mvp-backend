using System.Text.Json;
using Afrinet.API.Contracts;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Services.API.Service;

public class SqsPublisher
{
    private readonly IAmazonSQS _sqs;
    public SqsPublisher(IAmazonSQS sqs)
    {
        _sqs = sqs;

    }

    public async Task PublishAsync<T>(string queueName, T message) where T : IMessage
    {
        var queueUrl = await _sqs.GetQueueUrlAsync(queueName);
        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl.QueueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    nameof(IMessage.MessageTypeName),
                    new MessageAttributeValue {
                    DataType = "String",
                    StringValue = message.MessageTypeName
                     }
                },
                {
                    "timestamp",
                    new MessageAttributeValue {
                         StringValue = DateTime.Now.ToString(),
                         DataType = "String"
                          }
                },
                {
                    "version",
                    new MessageAttributeValue {
                         StringValue = "1.0",
                         DataType = "String"
                    }
                }
            }
        };

        await _sqs.SendMessageAsync(request);

    }

    public async Task PublishAsyncLocal<T>(string queueURL, T message) where T : IMessage
    {
        //var queueUrl = await _sqs.GetQueueUrlAsync(queueName);
        var request = new SendMessageRequest
        {
            QueueUrl =queueURL, // queueUrl.QueueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    nameof(IMessage.MessageTypeName),
                    new MessageAttributeValue {
                    DataType = "String",
                    StringValue = message.MessageTypeName
                     }
                },
                {
                    "timestamp",
                    new MessageAttributeValue {
                         StringValue = DateTime.Now.ToString(),
                         DataType = "String"
                          }
                },
                {
                    "version",
                    new MessageAttributeValue {
                         StringValue = "1.0",
                         DataType = "String"
                    }
                }
            }
        };

        await _sqs.SendMessageAsync(request);

    }
}