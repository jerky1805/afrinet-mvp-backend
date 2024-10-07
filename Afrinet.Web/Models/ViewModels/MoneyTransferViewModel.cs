using Afrinet.Models;

namespace IMT.Models;
public class MoneyTransferViewModel
{
    public string? Id { get; set; }
    public UserAccount userAccount { get; set; }
    public Transaction transaction { get; set; }

    public MoneyTransferViewModel()
    {
        userAccount = new();
        transaction = new();
    }
}