namespace AccountManagementService.Domain.Models;

public class Transaction
{
    public Guid TransactionId { get; set; }
    public string TransactionType { get; set; }
    public int? CreditAccountId { get; set; }
    public int? DebitAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }

    public Transaction(string transactionType, decimal amount, int? creditAccountId = null, int? debitAccountId = null)
    {
        TransactionType = transactionType;
        CreditAccountId = creditAccountId;
        DebitAccountId = debitAccountId;
        Amount = amount;
        TransactionDate = DateTime.Now;
    }

    public Transaction()
    {
    }
}