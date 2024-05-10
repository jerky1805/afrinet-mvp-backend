

using System.ComponentModel.DataAnnotations;

namespace Afrinet.Models;

public class PersonalDetails
{

    public string? Id { get; set; }
    [Required]
    public string? Names { get; set; }
    [Required]

    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Email { get; set; }
    public string? IDType { get; set; }
    public string? IDNumber { get; set; }
    public string? Language { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    [Required]

    public string? MSISDN { get; set; }
    public string? Status { get; set; }


}
