

namespace RAI.Models;

public class ServiceProvider
{

    public string? Id { get; set; }
    public string? OtherNames { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? WorkDescription { get; set; }
    public string? Category { get; set; }
    public string? Location { get; set; }
    public string? IDNumber { get; set; }
    public string? IDType { get; set; }
    public string? AgencyId { get; set; }
    public string? PaymentStatus { get; set; } //Paid, Fully Paid
    public string? VerificationStatus { get; set; } //Verified or Unverified
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; }


}
