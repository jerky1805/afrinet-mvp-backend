namespace Subscribers.API.Models;
public class LoanDetailsDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string LoanDetailsCollectionName { get; set; } = null!;
}