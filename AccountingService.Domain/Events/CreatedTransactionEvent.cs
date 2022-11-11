using AccountingService.Domain.Models.Enumerations;
using MediatR;

namespace AccountingService.Domain.Notifications;

public class CreatedTransactionEvent : INotification
{
    public Guid TransactionId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int? CreditAccountId { get; set; }
    public int? DebitAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
}