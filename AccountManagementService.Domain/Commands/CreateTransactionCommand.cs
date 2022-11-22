using MediatR;

namespace AccountManagementService.Domain.Commands;

public class CreateTransactionCommand : IRequest<object>
{
    public string TransactionType { get; set; }
    public int? CreditAccountId { get; set; }
    public int? DebitAccountId { get; set; }
    public decimal Amount { get; set; }
}