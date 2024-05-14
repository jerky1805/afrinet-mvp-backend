

namespace Afrinet.Models;

public class TrustAccount
{

    
    public string? Id { get; set; }
    public string? BankName { get; set; }
    public string? BankAccountNumber  { get; set; }
    public string? BankShortCode { get; set; }
    public string? BankBranchName { get; set; }
    public DateTime ReconciledAt { get; set; }
    public Wallet Wallet { get; set; } = new Wallet ();
    public string? Currency { get; set; } 
    public string? WalletId  { get; set; }

    public string? Name { get; set; }
    public DateTime CreatedAt  { get; set; }

    public long MaximumBalance {get; set;}
    public long MinimumBalance {get; set;}

     public long Balance {get; set;}
    public ServiceAccount ServiceAccount { get; set; } = new ServiceAccount();


}
