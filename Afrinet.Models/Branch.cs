
namespace Afrinet.Models;
public class Branch
{
    public string? Id { get; set; }
    public string? BranchName { get; set; }
    public string? MainContactName { get; set; }
    public string? MainContactOtherNames { get; set; }
    public string? MainContactMobileNumber { get; set; }
    public string? MainContactEmail { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? MSISDN { get; set; }
    public string? PIN { get; set; }

    public string? HeadOfficeId { get; set; }
    public ServiceAccount ServiceAccount { get; set; } = new ServiceAccount();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<TransactionLimit> TransactionLimits { get; set; } = new List<TransactionLimit>();
    public ValueLimit BalanceLimit { get; set; } = new ValueLimit();

    public List<Branch> Branches { get; set; } = new List<Branch>();
    public string? Status { get; set; }
    public string? ServiceAccountId { get; set; }

}
