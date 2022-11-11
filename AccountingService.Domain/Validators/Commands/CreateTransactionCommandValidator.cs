using AccountingService.Domain.Commands;
using FluentValidation;

namespace AccountingService.Domain.Validators.Commands;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor
    }
}