using FluentValidation;
using AccountingService.Api.Contracts.v1.Requests;
using static Credit.NetCore.Framework.Extensions.DocumentValidation;

namespace AccountingService.Api.Contracts.v1.Validators;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.Agency)
            .NotEmpty()
            .WithMessage("The Agency is required")
            
            .Length(3)
            .WithMessage("The Agency must have 3 digits")
            
            .Must(x => int.TryParse(x, out var val) && val > 0)
            .WithMessage("Invalid Agency number.");


        RuleFor(x => x.Document)
            .Cascade(FluentValidation.CascadeMode.Continue)

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