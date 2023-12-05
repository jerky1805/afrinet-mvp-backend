

namespace Afrinet.Models;
public class TrustBank
{

  
 public string? Id { get; set; }
    public string? CompanyName { get; set; }
    public string? AggregatorName { get; set; }
    public string? ContractSignatoryName { get; set; }

    public string? ContractSignatoryOtherNames { get; set; }
    public string? ContractSignatoryMobileNumber { get; set; }

    public string? ContractSignatoryEmail { get; set; }
    public string? MainContactName { get; set; }

    public string? MainContactOtherNames { get; set; }
    public string? MainContactMobileNumber { get; set; }

    public string? MainContactEmail { get; set; }

    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public bool SuperAgent { get; set; }

    public ServiceAccount ServiceAccount { get; set; } = new ServiceAccount();
}
