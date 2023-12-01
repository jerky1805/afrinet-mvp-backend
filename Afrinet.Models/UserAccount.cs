namespace Afrinet.Models;

public class UserAccount
{
    public string? Id { get; set; }
    public string? OtherNames { get; set; }
    public string? Surname { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Email { get; set; }
    public string? IDType { get; set; }
    public string? IDNumber { get; set; }
    public string? Language { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public ServiceAccount ServiceAccount { get; set; } = new ServiceAccount();
    public string? AccountType  { get; set; }

}
