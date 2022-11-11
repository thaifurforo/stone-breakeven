using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using FluentValidation;
using MediatR;

namespace AccountingService.Domain.CommandHandlers;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, string>
{
    private readonly IMediator _mediator;
    private readonly ITransactionRepository _repository;
    private readonly IAccountRepository _accountRepository;
    private readonly IValidator<CreateTransactionCommand> _validator;

    public CreateTransactionCommandHandler(IMediator mediator, ITransactionRepository repository, IAccountRepository accountRepository, IValidator<CreateTransactionCommand> validator)
    {
        _mediator = mediator;
        _repository = repository;
        _accountRepository = accountRepository;
        _validator = validator;
    }
    
    public async Task<string> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {

        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });
            return await Task.FromResult("There's been a validation error on the creation of the transaction");
        }
        
        try
        {
            var transaction = new Transaction(request.TransactionType, request.Amount, request.CreditAccountId, request.DebitAccountId);
            
            await _repository.AddTransaction(transaction);
            await _repository.Save();

            var creditAccount = _accountRepository.GetAccountById(request.CreditAccountId);
            var debitAccount = _accountRepository.GetAccountById(request.DebitAccountId);

            creditAccount.Balance += request.Amount;
            debitAccount.Balance -= request.Amount;

            await _accountRepository.UpdateAccount(creditAccount);
            await _accountRepository.UpdateAccount(debitAccount);
            
            await _mediator.Publish(new CreatedTransactionEvent {TransactionId = transaction.TransactionId, 
                TransactionType = transaction.TransactionType, CreditAccountId = transaction.CreditAccountId, 
                DebitAccountId = transaction.DebitAccountId, Amount = transaction.Amount, 
                TransactionDate = transaction.TransactionDate});

            return await Task.FromResult("Transaction successfully created");
        }
        
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });

            return await Task.FromResult("There's been an error on the creation of the transaction");
        }
    }
}