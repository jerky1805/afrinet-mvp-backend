namespace Subscribers.API.Models;
public class HeadOfficeAccountDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string HeadOfficeAccountCollectionName { get; set; } = null!;
}