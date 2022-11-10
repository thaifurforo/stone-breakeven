using Microsoft.EntityFrameworkCore;

namespace AccountingService.Domain.Models;

[Index(nameof(Number), IsUnique = true)]
public partial class Account
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public string Agency { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public string Document { get; set; }

    public static int GlobalId;

    public Account(string document, string agency, DateTime? closingDate = null, bool isActive = true, string? number = null)

    {
        Agency = agency;
        Number = number;
        Balance = 0;
        IsActive = isActive;
        OpeningDate = DateTime.Now;
        ClosingDate = closingDate;
        Document = document;
    }

    public Account()
    {
        
    }
    
}
    