

namespace Afrinet.Models;
public class Tax
{

    public string? Id { get; set; }
    public string? TaxName { get; set; }
    public long Amount { get; set; }
    public bool Settled { get; set; }
    public DateTime SettledDate { get; set; }
    public string? Currency { get; set; }
}
