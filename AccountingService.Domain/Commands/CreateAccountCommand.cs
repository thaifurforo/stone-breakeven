using MediatR;

namespace AccountingService.Domain.Commands;

public class CreateAccountCommand : IRequest<string>
{
    public string Agency { get; set; }
    public string Document { get; set; } 
}