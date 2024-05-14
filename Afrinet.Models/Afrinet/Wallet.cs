

namespace Afrinet.Models;
public class Wallet
{
    public string? Id { get; set; }
    public long Balance { get; set; }
    public string? Currency { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastOperatedAt { get; set; }
    public long MaximumBalance { get; set; }
    public long MinimumBalance { get; set; }
    public bool LowBalanceWarning { get; set; }
    public long DisputedBalance { get; set; }
    public string? ServiceAccountId { get; set; }

    public string? WalletType {get; set;}
}
