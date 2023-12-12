

namespace Afrinet.Models;

public class OrgAccount
{

    public string? Id { get; set; }
    public string? OrganisationName { get; set; }
    public string? MainContactEmail { get; set; }
    public string? MainContactName { get; set; }
    public string? MainContactOtherNames { get; set; }
    public string? MainContactMobileNumber { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? IDType { get; set; }
    public string? IDNumber { get; set; }
    public string? Language { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public ServiceAccount ServiceAccount { get; set; } = new ServiceAccount();
    public string? ServiceAccountId {get; set;}
    public string? AccountType { get; set; } //Agent , Subscriber
    public string? MSISDN { get; set; }
    public string? PIN { get; set; }
    public long FailedPINAttempts { get; set; }
    public DateTime PINChangedAt { get; set; }
    public string? UserAccountRole { get; set; }
    public List<TransactionLimit> TransactionLimits { get; set; } = new List<TransactionLimit>();
    public ValueLimit BalanceLimit { get; set; } = new ValueLimit();
    public List<string> Channels { get; set; } = new List<string>();
    public string? WebLoginID { get; set; }

            public string? Status { get; set; }


}
