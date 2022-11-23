using AccountManagementService.Domain.Commands;
using FluentValidation;

namespace AccountManagementService.Domain.Validators.Commands;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        When(x => x.TransactionType is "deposit", () =>
        {
            RuleFor(x => x.CreditAccountId)
                .NotEmpty()
                .WithMessage("A Credit Account must be informed for this transaction.");
            RuleFor(x => x.DebitAccountId)
                .Empty()
                .WithMessage("There should be no Debit Account informed for deposit transactions.");
        });
        
        When(x => x.TransactionType is "transfer", () =>
        {
            RuleFor(x => x.CreditAccountId)
                .NotEmpty()
                .WithMessage("A Credit Account must be informed for this transaction.");
            RuleFor(x => x.DebitAccountId)
                .NotEmpty()
                .WithMessage("A Debit Account must be informed for this transaction.");
        });
        
        When(x => x.TransactionType is "withdraw", () =>
        {
            RuleFor(x => x.CreditAccountId)
                .Empty()
                .WithMessage("There should be no Credit Account informed for withdraw transactions.");
            RuleFor(x => x.DebitAccountId)
                .NotEmpty()
                .WithMessage("A Debit Account must be informed for this transaction.");
        });
        
        When(x => x.DebitAccountId is not null, () =>
        {
            RuleFor(x => x.DebitAccountId)
                .NotEqual(x => x.CreditAccountId)
                .WithMessage("Credit and Debit accounts can't be the same.");
        });

        var transactionTypes = new List<string>() { "deposit", "withdraw", "transfer" };
        RuleFor(x => x.TransactionType)
            .NotEmpty()
            .NotNull()
            .Must(x => transactionTypes.Contains(x))
            .WithMessage("The transaction type must be one of the following: deposit, transfer or withdraw.");
    }
}