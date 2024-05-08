

namespace Afrinet.Models;

public class LoanDetails
{

    public string? Id { get; set; }
    public string? LoanPurpose { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public decimal Interest { get; set; }
    public int LoanTenure { get; set; }
    public string? TenureSchedule { get; set; }
    public CompanyDetails companyDetails { get; set; } = new CompanyDetails();
    public PersonalDetails personalDetails { get; set; } = new PersonalDetails();

}
