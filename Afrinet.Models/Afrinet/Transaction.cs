
namespace Afrinet.Models;
public class Transaction
{

    public string? Id { get; set; }
    public string? InitatingAccountID { get; set; }

    public string? SendingAccountID { get; set; }

    public string? ReceivingAccountID { get; set; }

    public string? ReceivingMSISDN { get; set; }


    public DateTime StartedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }
    public DateTime CompletedAt { get; set; }
    public string? State { get; set; }

    public string? TransactionType { get; set; }

    public long ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }
    public bool IsReversed { get; set; }

    public string? ReversedTransactionID { get; set; }

    public double TotalFees { get; set; }

    public long TotalTax { get; set; }

    public List<Fee> Fees { get; set; } = new List<Fee>();

    public List<Tax> Taxes { get; set; } = new List<Tax>();

    public string? Channel { get; set; }
    public double Amount { get; set; }

    public Voucher? Voucher { get; set; }



}
