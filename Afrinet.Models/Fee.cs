
namespace Afrinet.Models;
public class Fee
{
  
    public string? Id { get; set; }
    public string? FeeName { get; set; }
    public DateTime CreatedAt { get; set; }

    public bool Collected { get; set; }

    public DateTime CollectedAt { get; set; }

    public DateTime PayerServiceAccountID { get; set; }

    public List<Commission> Commissions { get; set; } = new List<Commission>();

}
