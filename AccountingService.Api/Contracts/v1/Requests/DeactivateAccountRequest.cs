using AccountingService.Api.Contracts.v1.Response;
using AccountingService.Api.Contracts.v1.Validators;

namespace AccountingService.Api.Contracts.v1.Requests;

public class DeactivateAccountRequest
{
    
    public DateOnly OpeningDate { get; set; }
    public DateOnly ClosingDate { get; set; }
    public decimal Balance { get; set; }

    public DeactivateAccountRequest(int id)
    {
        var account = AccountList.Accounts.Find(account => account.Id == id);
        account.Status = false;
        account.ClosingDate = DateOnly.FromDateTime(DateTime.Now);
    }

    public void Validate()
    {
        var validationResult = new DeactivateAccountRequestValidator().Validate(this);

        if (!validationResult.IsValid)
        {
            throw new BadHttpRequestException($"Invalid request: {validationResult}. Please, try again.");
        }
    }
}