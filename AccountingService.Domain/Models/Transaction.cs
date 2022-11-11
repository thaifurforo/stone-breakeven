using AccountingService.Domain.Models.Enumerations;

namespace AccountingService.Domain.Models;

public class Transaction
{
    public Guid TransactionId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int? CreditAccountId { get; set; }
    public int? DebitAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }

    public Transaction(TransactionType transactionType, decimal amount, int? creditAccountId = null, int? debitAccountId = null)
    {
        TransactionType = transactionType;
        CreditAccountId = creditAccountId;
        DebitAccountId = debitAccountId;
        Amount = amount;
        TransactionDate = DateTime.Now;
    }
}