

namespace AccountingService.Domain.Model;

public class Account
{
    public string Number { get; set; }
    public string Agency { get; set; }
    public decimal Amount { get; set; }
    public bool Status { get; set; }
    public DateOnly OpeningDate { get; set; }
    public DateOnly? ClosingDate { get; set; }
    public string Document { get; set;  }

    public Account(string document, string agency, string number,
        decimal amount = 0, DateOnly? closingDate = null, bool status = true)

    {

        Agency = agency;
        Number = AccountNumberGenerator.GetAccountNumber();
        Amount = amount;
        Status = status;
        OpeningDate = DateOnly.FromDateTime(DateTime.Now);
        ClosingDate = closingDate;
        Document = document;

    }

    public void VerifyIfDocumentValid(string document)
    {
        if (!((document.Length == 11 && document.IsValidCpf()) || (document.Length == 14 && document.isValidCnpj()))
            // Credit.NetCore.Framework.Extensions.DocumentValidation - copiar lib
            || (document.Length != 11 && document.Length != 14))
        {
            throw new Exception();
            // COLOCAR A EXCEPTION ESPECÍFICA AQUI
        }
    }
}