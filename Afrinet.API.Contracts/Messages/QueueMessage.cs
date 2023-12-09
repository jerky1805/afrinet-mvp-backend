using System.Text.Json.Serialization;

namespace Afrinet.API.Contracts;

public class QueueMessage : IMessage
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("type")]
    public int Type { get; set; }
    [JsonPropertyName("order")]
    public string MessagePayload { get; set; }
    [JsonIgnore]
    public string MessageTypeName => nameof(QueueMessage);
}