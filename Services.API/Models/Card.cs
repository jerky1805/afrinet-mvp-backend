
namespace Services.API.Models;
public class Card
{

    public string? Id { get; set; }
    public string? Country { get; set; }
    public string? Currency { get; set; }
    public string? CardToken { get; set; }
    public string? CardNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public string? CardType { get; set; } //Debit, Credit
    public string? FirstSixDigits { get; set; }
    public string? LastFourDigits { get; set; }
    public string? CVV { get; set; }
    public string? ExpiryMonth { get; set; }
    public string? ExpiryYear { get; set; }
    public string? AuthOption { get; set; } // 3DS flag 

}
