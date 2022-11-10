using MediatR;

namespace AccountingService.Domain.Commands;

public class DeactivateAccountCommand : IRequest<string>
{
    public int Id { get; set; }
    public DateTime ClosingDate { get; set; }
    public bool IsActive { get; set; }
}