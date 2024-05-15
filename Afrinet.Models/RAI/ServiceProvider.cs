

namespace RAI.Models;

public class RAIServiceProvider
{

    public string? Id { get; set; }
    public string? Name { get; set; }
    public ContactPerson? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? WorkDescription { get; set; }
    public string? ServiceId { get; set; }
    public string? LocationId { get; set; }
    public string? IDNumber { get; set; }
    public string? IDType { get; set; }
    public PhotoID? PhotoID { get; set; }
    public string? AgencyId { get; set; }

    public string? AgentId { get; set; }

    public string? PaymentStatus { get; set; } //Paid, Fully Paid
    public string? VerificationStatus { get; set; } //Verified or Unverified
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; }


}


public class ContactPerson
{

    public string? OtherNames { get; set; }
    public string? Surname { get; set; }

}

public class PhotoID
{

    public string? FrontURL { get; set; }
    public string? BackURL { get; set; }

}