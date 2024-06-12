namespace Subscribers.Models;

public class ExchangeValue
{
    public double Amount { get; set; }

    public string Base { get; set; }

    public string Date { get; set; }

    //public List<string> Rates { get; set; }

    public Dictionary<string, double> Rates { get; set; }

}