using System.Text.Json.Serialization;
using DateOnlyTimeOnly.AspNet.Converters;

namespace AccountingService.Domain.Models;

public partial class Account
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Agency { get; set; }
    public decimal Balance { get; set; }
    public bool Status { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public string Document { get; set; }


    public Account(string document, string agency, DateTime? _closingDate = null, bool _status = true)

    {
        Agency = agency;
        Number = GetAccountNumber();
        Balance = 0;
        Status = _status;
        OpeningDate = DateTime.Now;
        ClosingDate = _closingDate;
        Document = document;
    }

    public Account()
    {
        
    }
    
}
    