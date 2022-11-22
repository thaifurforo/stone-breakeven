using AccountManagementService.Domain.Commands;
using FluentValidation;

namespace AccountManagementService.Domain.Validators.Commands;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.CreditAccountId)
            .NotEmpty()
            .When(x => x.TransactionType == "deposit")
            .WithMessage("A Credit Account must be informed for deposit transactions.")

            .NotEmpty()
            .When(x => x.TransactionType == "transfer")
            .WithMessage("A Credit Account must be informed for transfer between accounts transactions.")

            .Empty()
            .When(x => x.TransactionType == "withdraw")
            .WithMessage("There should be no Credit Account informed for withdraw transactions.");

        RuleFor(x => x.DebitAccountId)
            .NotEmpty()
            .When(x => x.TransactionType == "withdraw")
            .WithMessage("A Debit Account must be informed for withdraw transactions.")

            .NotEmpty()
            .When(x => x.TransactionType == "transfer")
            .WithMessage("A Debit Account must be informed for transfer between accounts transactions.")

            .Empty()
            .When(x => x.TransactionType == "deposit")
            .WithMessage("There should be no Debit Account informed for deposit transactions.")

            .NotEqual(x => x.CreditAccountId)
            .WithMessage("Credit and Debit accounts can't be the same.");

        var transactionTypes = new List<string>() { "deposit", "withdraw", "transfer" };
        RuleFor(x => x.TransactionType)
            .NotEmpty()
            .Must(x => transactionTypes.Contains(x))
            .WithMessage("The transaction type must be one of the following: deposit, transfer or withdraw.");
    }
}