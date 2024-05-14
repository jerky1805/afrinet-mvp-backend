
using System.ComponentModel.DataAnnotations;

namespace Afrinet.Models;

public class ConfirmationRequest
{
    public string? Id { get; set; }
    [Required]
    public string? SubscriberId { get; set; }
    [Required]
    [Phone]
    public string? MSISDN { get; set; }
    public string? TransactionId { get; set; }
    public string? PIN { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Status { get; set; }

}
