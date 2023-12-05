

namespace Afrinet.Models;
public class ServiceAccount
{

    public string? Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public List<Service> SupportedServices { get; set; } = new List<Service>();
    public List<Wallet> Wallets { get; set; } = new List<Wallet>();
}
