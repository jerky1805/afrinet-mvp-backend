

namespace Afrinet.Models.Payment;

public class PaymentCustomer
{

    public string? Id { get; set; }
    public string? OtherNames { get; set; }
    public string? Surname { get; set; }
    public string? CompanyName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public ServiceAccount ServiceAccount { get; set; } = new ServiceAccount();

    public string? ServiceAccountId { get; set; }
    public string? CustomerType { get; set; } //Organisation , Individual
    public string? MobileNumber { get; set; }
    public string? PIN { get; set; }
    public string? WebLoginID { get; set; }

}
