using AccountingService.Api.Contracts.v1.Validators;

namespace AccountingService.Api.Contracts.v1.Requests;

public class CreateAccountRequest
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Agency { get; set; }
    public decimal Amount { get; set; }
    public bool Status { get; set; }
    public DateOnly OpeningDate { get; set; }
    public DateOnly? ClosingDate { get; set; }
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