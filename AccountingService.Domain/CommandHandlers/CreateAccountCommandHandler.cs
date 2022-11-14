using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using FluentValidation;
using MediatR;
namespace AccountingService.Domain.CommandHandlers;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, string>
{
    private readonly IMediator _mediator;
    private readonly IAccountRepository _repository;
    private readonly IValidator<CreateAccountCommand> _validator;

    public CreateAccountCommandHandler(IMediator mediator, IAccountRepository repository, IValidator<CreateAccountCommand> validator)
    {
        _mediator = mediator;
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<string> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {

        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });
            return await Task.FromResult($"There's been a validation error on the creation of the account: {ex.Message}");
        }
        
        try
        {
            var account = new Account(request.Document, request.Agency);

            await _repository.AddAccount(account);
            await _repository.Save();
            
            account.Number = account.GetAccountNumber();
            await _repository.UpdateAccount(account);
            await _repository.Save();
            
            await _mediator.Publish(new CreatedAccountEvent { Id = account.Id, Number = account.Number, 
                Agency = account.Agency, Balance = account.Balance, IsActive = account.IsActive, 
                OpeningDate = account.OpeningDate, ClosingDate = account.ClosingDate, Document = account.Document});

            return await Task.FromResult("Account successfully created");
        }
        
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });

            return await Task.FromResult($"There's been an error on the creation of the account: {ex.Message}");
        }
    }
}