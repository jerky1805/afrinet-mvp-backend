namespace Subscribers.API.Models;
public class ServiceAccountDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ServiceAccountCollectionName { get; set; } = null!;
}