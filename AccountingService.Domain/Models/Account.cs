using System.Text.Json.Serialization;
using DateOnlyTimeOnly.AspNet.Converters;

namespace AccountingService.Domain.Models;

public partial class Account
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Agency { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public string Document { get; set; }

    public static int GlobalId;

    public Account(string document, string agency, DateTime? _closingDate = null, bool _isActive = true)

    {
        Agency = agency;
        Number = GetAccountNumber();
        Balance = 0;
        IsActive = _isActive;
        OpeningDate = DateTime.Now;
        ClosingDate = _closingDate;
        Document = document;
    }

    public Account()
    {
        
    }
    
}
    