using AccountManagementService.Domain.Commands;
using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Models;
using AccountManagementService.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AccountManagementService.Domain.CommandHandlers;

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
        catch (NullReferenceException ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });
            throw new NullReferenceException($"There's been an error on the deactivation of the account: AccountId doesn't exist on the database");
        }
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });
            throw new BadHttpRequestException($"There's been an error on the deactivation of the account: {ex.Message}");
        }
    }
}