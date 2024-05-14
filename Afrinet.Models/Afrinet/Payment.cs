
namespace Afrinet.Models.Payment;
public class Payment
{

    public string? Id { get; set; }
    public string? Reference { get; set; }
    public string? ProductName { get; set; }
    public string? TotalAmountCharged { get; set; }
    public string? Currency { get; set; }
    public string? PaymentStatus { get; set; }
    public string? PaymentMethod { get; set; }
    public string? PaymentResponseCode { get; set; }
    public string? PaymentResponseMessage { get; set; }
    public string? Narration { get; set; }
    public string? Remarks { get; set; }
    public string? CustomerId { get; set; }
    public string? SendingAccountID { get; set; }
    public string? ReceivingAccountID { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public DateTime CompletedAt { get; set; }
    public string? PaymentType { get; set; }
    public long ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsReversed { get; set; }
    public string? ReversedPaymentID { get; set; }
    public long TotalFees { get; set; }
    public long TotalTax { get; set; }
    public List<Fee> Fees { get; set; } = new List<Fee>();
    public List<Tax> Taxes { get; set; } = new List<Tax>();
    public string? Channel { get; set; }
    public long Amount { get; set; }
    public PaymentCustomer paymentCustomer { get; set;} = new PaymentCustomer();
    public PaymentCard paymentCard{ get; set; } = new PaymentCard();
    public Order order { get; set; } = new Order();

}
