

using System.ComponentModel.DataAnnotations;

namespace Afrinet.Models;

public class LoanDetails
{

    public string? Id { get; set; }
    [Required]

    public string? LoanPurpose { get; set; }

    [Required]
    public decimal LoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public decimal Interest { get; set; }
    [Range(1, 6)]
    public int LoanTenure { get; set; }
    [Required]

    public string? TenureSchedule { get; set; }
    public CompanyDetails companyDetails { get; set; } = new CompanyDetails();
    public PersonalDetails personalDetails { get; set; } = new PersonalDetails();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


}
