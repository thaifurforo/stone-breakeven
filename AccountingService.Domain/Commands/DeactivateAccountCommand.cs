using MediatR;

namespace AccountingService.Domain.Commands;

public class DeactivateAccountCommand : IRequest<object>
{
    public int Id { get; set; }
    public DateTime ClosingDate { get; set; }
}