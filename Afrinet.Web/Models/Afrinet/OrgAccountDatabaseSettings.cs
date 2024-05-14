namespace Subscribers.API.Models;
public class OrgAccountDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string OrgAccountCollectionName { get; set; } = null!;
}