namespace AccountingService.Domain.Model;

public class Account
{
    public string Agency { get; set; }
    public string Number { get; set; }
    public decimal Amount { get; set; }
    public bool Status { get; set; }
    public DateOnly OpeningDate { get; set; }
    public DateOnly? ClosingDate { get; set; }
    public string Document { get; set;  }

    public Account(string document, DateOnly closingDate, string agency, bool status = true, string Number = "000001-2")
    {

        Agency = agency;
        Number = 
        OpeningDate = DateOnly.FromDateTime(DateTime.Now);
    }
}