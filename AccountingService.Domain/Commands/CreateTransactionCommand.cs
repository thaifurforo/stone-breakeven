using MediatR;

namespace AccountingService.Domain.Commands;

public class CreateTransactionCommand : IRequest<string>
{
    public string TransactionType { get; set; }
    public int? CreditAccountId { get; set; }
    public int? DebitAccountId { get; set; }
    public decimal Amount { get; set; }
}