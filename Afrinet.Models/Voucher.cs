

namespace Afrinet.Models;
public class Voucher
{

   
    public string? Id { get; set; }
    public string? TransactionId { get; set; }
    public string? PayeeMSISDN { get; set; }
    public long Amount { get; set; }
    public string? Currency { get; set; }

    public string? State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }

}
