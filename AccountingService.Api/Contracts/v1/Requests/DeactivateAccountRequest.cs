using AccountingService.Api.Contracts.v1.Validators;

namespace AccountingService.Api.Contracts.v1.Requests;

public class DeactivateAccountRequest
{
    
    public DateOnly OpeningDate { get; }
    public DateOnly ClosingDate { get; }

    public void Validate()
    {
        var validationResult = new DeactivateAccountRequestValidator().Validate(this);

        if (!validationResult.IsValid)
        {
            throw new BadHttpRequestException($"Invalid request: {validationResult}. Please, try again.");
        }
    }
}