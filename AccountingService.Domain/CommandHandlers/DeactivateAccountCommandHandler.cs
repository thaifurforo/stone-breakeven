using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using MediatR;
using Newtonsoft.Json;

namespace AccountingService.Domain.CommandHandlers;

public class DeactivateAccountCommandHandler : IRequestHandler<DeactivateAccountCommand, object>
{
    private readonly IMediator _mediator;
    private readonly IAccountRepository _repository;

    public DeactivateAccountCommandHandler(IMediator mediator, IAccountRepository repository)
    {
        this._mediator = mediator;
        this._repository = repository;
    }

    public async Task<object> Handle(DeactivateAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.DeactivateAccount(request.Id);
            await _repository.Save();

            var account = _repository.GetAccountById(request.Id).Result;
            
            await _mediator.Publish(new DeactivatedAccountEvent { Id = account.Id, Number = account.Number, 
                Agency = account.Agency, Balance = account.Balance, IsActive = account.IsActive, 
                OpeningDate = account.OpeningDate, ClosingDate = account.ClosingDate, Document = account.Document});
            return await Task.FromResult(account);

        }
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });
            return await Task.FromResult($"There's been an error on the deactivation of the account: {ex.Message}");
        }
    }
}