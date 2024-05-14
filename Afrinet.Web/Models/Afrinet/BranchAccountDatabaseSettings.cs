namespace Subscribers.API.Models;
public class BranchAccountDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string BranchAccountCollectionName { get; set; } = null!;
}