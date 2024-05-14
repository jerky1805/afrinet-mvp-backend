namespace Subscribers.API.Models;
public class WalletDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string WalletCollectionName { get; set; } = null!;
}