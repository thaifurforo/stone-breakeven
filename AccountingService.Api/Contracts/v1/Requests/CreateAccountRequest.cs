using AccountingService.Api.Contracts.v1.Validators;

namespace AccountingService.Api.Contracts.v1.Requests;

public class CreateAccountRequest
{
    public string Agency { get; set; }
    public string Document { get; set; }

    public CreateAccountRequest(string agency, string document)
    {
        Agency = agency;
        Document = document;
    }

    public void Validate()
    {
        var validationResult = new CreateAccountRequestValidator().Validate(this);

        if (!validationResult.IsValid)
        {
            throw new BadHttpRequestException($"Invalid request: {validationResult}. Please, try again.");
        }
        
    }
}