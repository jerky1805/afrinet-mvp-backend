namespace Services.API.Models;
public class ConfirmationDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ConfirmationCollectionName { get; set; } = null!;
}