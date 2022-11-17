using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AccountingService.Domain.CommandHandlers;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, object>
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
    
    public async Task<object> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {

        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });
            return await Task.FromResult($"There's been a validation error on the creation of the transaction: {ex.Message}");
        }
        
        try
        {
            var transaction = new Transaction(request.TransactionType, request.Amount, request.CreditAccountId, request.DebitAccountId);
            
            await _repository.AddTransaction(transaction);
            await _repository.Save();

            if (request.CreditAccountId != null)
            {
                var creditAccount = _accountRepository.GetAccountById(request.CreditAccountId).Result;
                if (!creditAccount.IsActive)
                {
                    throw new BadHttpRequestException(message: "The Credit Account informed is not active. Try again.");
                }
                else
                {
                    creditAccount.Balance += request.Amount;
                    creditAccount.Transactions.Add(transaction);
                    await _accountRepository.UpdateAccount(creditAccount);
                    await _accountRepository.Save();
                }
            }

            if (request.DebitAccountId != null)
            {
                var debitAccount = _accountRepository.GetAccountById(request.DebitAccountId).Result;
                if (!debitAccount.IsActive)
                {
                    throw new BadHttpRequestException(message: "The Debit Account informed is not active. Try again.");
                }
                else
                {
                    debitAccount.Balance -= request.Amount;
                    debitAccount.Transactions.Add(transaction);
                    await _accountRepository.UpdateAccount(debitAccount);
                    await _accountRepository.Save();
                }
            }
            
            await _mediator.Publish(new CreatedTransactionEvent {TransactionId = transaction.TransactionId, 
                TransactionType = transaction.TransactionType, CreditAccountId = transaction.CreditAccountId, 
                DebitAccountId = transaction.DebitAccountId, Amount = transaction.Amount, 
                TransactionDate = transaction.TransactionDate});

            return await Task.FromResult(transaction);
        }
        
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorEvent { Exception = ex.Message, ErrorPile = ex.StackTrace });

            return await Task.FromResult($"There's been an error on the creation of the transaction: {ex.Message}");
        }
    }
}