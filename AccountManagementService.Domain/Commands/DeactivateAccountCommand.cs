using MediatR;

namespace AccountManagementService.Domain.Commands;

public class DeactivateAccountCommand : IRequest<object>
{
    public int Id { get; set; }
}