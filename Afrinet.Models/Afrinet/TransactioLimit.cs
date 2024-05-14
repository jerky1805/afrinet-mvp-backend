
namespace Afrinet.Models;
public class TransactionLimit
{

     public string? Id { get; set; }
    public string? LimitName { get; set; }
    public string? LimitType { get; set; }// Fixed Value, Band, Velocity
    public long MaximumValue { get; set; }
    public long MinimumValue { get; set; }
    public int Count { get; set; }
    public int Duration { get; set; }


}
