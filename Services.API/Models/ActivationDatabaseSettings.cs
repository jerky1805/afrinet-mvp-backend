namespace Services.API.Models;
public class ActivationDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ActivationCollectionName { get; set; } = null!;
}