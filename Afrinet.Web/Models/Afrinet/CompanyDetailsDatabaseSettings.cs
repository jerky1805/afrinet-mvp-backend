namespace Subscribers.API.Models;
public class CompanyDetailsDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string CompanyDetailsCollectionName { get; set; } = null!;
}