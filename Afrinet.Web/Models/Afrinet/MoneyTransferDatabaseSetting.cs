namespace Subscribers.API.Models;
public class MoneyTransferDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string MoneyTransferCollectionName { get; set; } = null!;
}