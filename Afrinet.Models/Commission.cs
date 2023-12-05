

namespace Afrinet.Models;
public class Commission
{
   
    public string? Id { get; set; }
    public string? FeeName { get; set; }

    public string? FeeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Settled { get; set; }

    public DateTime SettledAt { get; set; }

    public long TaxPaid { get; set; }

    public string? ReceivingServiceAccountId { get; set; }


}
