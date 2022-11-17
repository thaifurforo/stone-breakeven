using AccountingService.Domain.Commands;
using AccountingService.Domain.Extensions;
using FluentValidation;

namespace AccountingService.Domain.Validators.Commands;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Agency)
            .NotEmpty()
            .WithMessage("The Agency is required")

            .Length(3)
            .WithMessage("The Agency must have 3 digits")

            .Must(x => int.TryParse(x, out var val) && val > 0)
            .WithMessage("Invalid Agency number");


        RuleFor(x => x.Document)
            .Cascade(CascadeMode.Continue)

            .NotEmpty()
            .WithMessage("The Document is required")

            .Length(14)
            .When(x => x.Document.Length != 11 && x.Document.Length != 14, ApplyConditionTo.CurrentValidator)
            .WithMessage("Document must have 11 (CPF) or 14 (CNPJ) characters")

            .Must(x => x.IsValidCpf())
            .When(x => x.Document.Length == 11, ApplyConditionTo.CurrentValidator)
            .WithMessage("Document: Invalid CPF")

            .Must(x => x.IsValidCnpj())
            .When(x => x.Document.Length == 14, ApplyConditionTo.CurrentValidator)
            .WithMessage("Document: Invalid CNPJ");
    }
}