using MediatR;

namespace AccountingService.Domain.Commands;

public class CreateAccountCommand : IRequest<object>
{
    public string Agency { get; set; }
    public string Document { get; set; } 
}