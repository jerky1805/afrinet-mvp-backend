
namespace Services.API.Models;

public class PaymentOrder
{
    public string? Id { get; set; }
    public string? Reference { get; set; }
    public string? Description { get; set; }
    public string? ProductId { get; set; }
    public long Amount { get; set; }
    public string? Currency { get; set; }
    public long Fee { get; set; }
    public string? Status { get; set; }
    public string? Instruction { get; set; }

}
