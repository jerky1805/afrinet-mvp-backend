
using Afrinet.Models;
using Afrinet.Models.Payment;

namespace Services.API.Models;
public class OrderDetails
{
    public string? Id { get; set; }
    public string? Reference { get; set; }
    public string? Description { get; set; }
    public string? ProductId { get; set; }
    public long Amount { get; set; }
    public string? Currency { get; set; }
    public PaymentOrder order { get; set; } = new PaymentOrder();
    public PaymentCustomer paymentCustomer { get; set; } = new PaymentCustomer();
    public List<PaymentOption> paymentOptions{ get; set; } = new List<PaymentOption>();

}
