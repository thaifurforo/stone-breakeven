using System;

namespace AccountingService.Domain.Model;

public class Account
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Agency { get; set; }
    public decimal Amount { get; set; }
    public bool Status { get; set; }
    public DateOnly OpeningDate { get; set; }
    public DateOnly? ClosingDate { get; set; }
    public string Document { get; set;  }

    public Account(string document, string agency, decimal amount = 0, DateOnly? closingDate = null, bool status = true)

    {
        Agency = agency;
        Number = AccountNumberGenerator.GetAccountNumber(Id);
        Amount = amount;
        Status = status;
        OpeningDate = DateOnly.FromDateTime(DateTime.Now);
        ClosingDate = closingDate;
        Document = document;
    }
    
}