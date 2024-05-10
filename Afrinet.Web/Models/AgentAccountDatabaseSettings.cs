namespace Subscribers.API.Models;
public class AgentAccountDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string AgentAccountCollectionName { get; set; } = null!;
}