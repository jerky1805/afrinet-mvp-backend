namespace Services.API.Models;
public class TransactionDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string TransactionCollectionName { get; set; } = null!;
}