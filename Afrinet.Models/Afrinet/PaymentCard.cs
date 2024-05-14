
namespace Afrinet.Models.Payment;
public class PaymentCard
{

    public string? Id { get; set; }
    public string? Country { get; set; }
    public string? Currency { get; set; }
    public string? CustomerId { get; set; }
    public string? CardToken { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CardNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public string? CardType { get; set; } //Debit, Credit
    public string? FirstSixDigits { get; set; }
    public string? LastFourDigits { get; set; }

}
