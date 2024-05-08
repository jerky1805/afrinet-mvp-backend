

namespace Afrinet.Models;

public class CompanyDetails
{

    public string? Id { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public List<string> CompanyDocuments { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


}
