

namespace RAI.Models;

public class CommissionSetting
{

    public string? Id { get; set; }
    public decimal RegistrationFee { get; set; }
    public int AgentCommission { get; set; }

    public int AgencyCommission { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


}
