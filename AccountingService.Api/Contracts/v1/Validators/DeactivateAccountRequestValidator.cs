using FluentValidation;
using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Domain.Models;

namespace AccountingService.Api.Contracts.v1.Validators;

public class DeactivateAccountRequestValidator : AbstractValidator<DeactivateAccountRequest>
{
    public DeactivateAccountRequestValidator()
    {
        RuleFor(x => x.ClosingDate)

            .GreaterThanOrEqualTo(x => x.OpeningDate)
            .WithMessage("Closing date must be equal or greater than opening date");

        RuleFor(x => x.Balance)
            .Equal(0)
            .WithMessage("It's impossible to deactivate an account whose balance is different from 0");
    }
}