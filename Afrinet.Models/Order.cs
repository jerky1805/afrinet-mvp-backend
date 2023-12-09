
namespace Afrinet.Models.Payment;
public class Order
{
    public string? Id { get; set; }
    public string? Reference { get; set; }
    public string? Description { get; set; }
    public string? ProductId { get; set; }
    public long Amount { get; set; }
    public string? Currency { get; set; }

}
