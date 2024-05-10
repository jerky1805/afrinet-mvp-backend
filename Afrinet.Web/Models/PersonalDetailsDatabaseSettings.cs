namespace Subscribers.API.Models;
public class PersonalDetailsDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string PersonalDetailsCollectionName { get; set; } = null!;
}