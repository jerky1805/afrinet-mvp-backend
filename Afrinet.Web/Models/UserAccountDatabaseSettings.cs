namespace Subscribers.API.Models;
public class UserAccountDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string UserAccountCollectionName { get; set; } = null!;
}