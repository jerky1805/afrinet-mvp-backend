using System.ComponentModel.DataAnnotations;

namespace Afrinet.Models;
public class HeadOffice
{

    public string? Id { get; set; }
    [Required]
    public string? CompanyName { get; set; }
    public string? AggregatorName { get; set; }
    [Required]
    public string? ContractSignatoryName { get; set; }
    [Required]
    public string? ContractSignatoryOtherNames { get; set; }
    [Phone]
    [Required]
    public string? ContractSignatoryMobileNumber { get; set; }
    [Required]
    [EmailAddress]
    public string? ContractSignatoryEmail { get; set; }
    [Required]
    public string? MainContactName { get; set; }
    [Required]
    public string? MainContactOtherNames { get; set; }
    [Required]
    [Phone]
    public string? MainContactMobileNumber { get; set; }
    [Required]
    [EmailAddress]
    public string? MainContactEmail { get; set; }
    [Required]
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    [Required]
    public string? City { get; set; }
    public string? State { get; set; }
    [Required]
    public string? Country { get; set; }
    public bool SuperAgent { get; set; }
    [Required]
    public string? MSISDN { get; set; }
    public string? PIN { get; set; }
    public List<TransactionLimit> TransactionLimits { get; set; } = new List<TransactionLimit>();
    public ValueLimit BalanceLimit { get; set; } = new ValueLimit();
    public List<Branch> Branches { get; set; } = new List<Branch>();
    public ServiceAccount ServiceAccount { get; set; } = new ServiceAccount();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Status { get; set; }
    public string? ServiceAccountId { get; set; }


}
