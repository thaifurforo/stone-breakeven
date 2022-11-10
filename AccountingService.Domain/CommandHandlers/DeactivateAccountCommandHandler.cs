using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Notifications;
using MediatR;

namespace AccountingService.Domain.CommandHandlers;

public class DeactivateAccountCommandHandler : IRequestHandler<DeactivateAccountCommand, string>
{
    private readonly IMediator _mediator;
    private readonly IAccountRepository _repository;

    public DeactivateAccountCommandHandler(IMediator mediator, IAccountRepository repository)
    {
        this._mediator = mediator;
        this._repository = repository;
    }

    public async Task<string> Handle(DeactivateAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.DeactivateAccount(request.Id);
            await _repository.Save();
            
            await _mediator.Publish(new DeactivatedAccountEvent { Id = request.Id, IsDeactivated = true });
            return await Task.FromResult("Account successfully deactivated");

        }
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });
            return await Task.FromResult("There's been an error on the deactivation of the account");
        }
    }
}