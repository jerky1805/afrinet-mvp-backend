

namespace RAI.Models;

public class Staff
{

    public string? Id { get; set; }
    public string? OtherNames { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Status { get; set; } //Active, Inactive
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string? UserLoginId { get; set; } 
}
