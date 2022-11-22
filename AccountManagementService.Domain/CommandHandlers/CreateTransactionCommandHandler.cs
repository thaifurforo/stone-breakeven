using AccountManagementService.Domain.Commands;
using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Models;
using AccountManagementService.Domain.Notifications;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AccountManagementService.Domain.CommandHandlers;

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
            
            var transaction = new Transaction(request.TransactionType, request.Amount, request.CreditAccountId, request.DebitAccountId);
            
            await _repository.AddTransaction(transaction);
            await _repository.Save();

            if (request.CreditAccountId != null)
            {
                var creditAccount = await _accountRepository.GetAccountById(request.CreditAccountId);
                if (creditAccount != null)
                {
                    if (!creditAccount.IsActive)
                    {
                        throw new BadHttpRequestException(
                            message: "The Credit Account informed is not active. Try again.");
                    }

                    creditAccount.Balance += request.Amount;
                    creditAccount.Transactions.Add(transaction);
                    await _accountRepository.UpdateAccount(creditAccount);
                    await _accountRepository.Save();
                }
                else
                {
                    throw new BadHttpRequestException(
                        message: "The CreditAccountId informed doesn't exist in the database.");
                }
            }

            if (request.DebitAccountId != null)
            {
                var debitAccount = _accountRepository.GetAccountById(request.DebitAccountId).Result;
                if (debitAccount != null)
                {
                    if (!debitAccount.IsActive)
                    {
                        throw new BadHttpRequestException(
                            message: "The Debit Account informed is not active. Try again.");
                    }
                    debitAccount.Balance -= request.Amount;
                    debitAccount.Transactions.Add(transaction);
                    await _accountRepository.UpdateAccount(debitAccount);
                    await _accountRepository.Save();
                }
                else
                {
                    throw new BadHttpRequestException(
                        message: "The DebitAccountId informed doesn't exist in the database.");
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

            throw new BadHttpRequestException($"There's been an error on the creation of the transaction: {ex.Message}");
        }
    }
}