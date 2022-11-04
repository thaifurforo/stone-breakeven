using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Contracts.v1.Validators;
using FluentAssertions;

namespace AccountingService.UnitTests.Api.Contracts.v1.Validators;

public class CreateAccountRequestValidatorTests
{
    private readonly CreateAccountRequestValidator _validator = new();
    
    public static readonly object[][] TheoryData =
    {
        new object[] { "001", "12345678909", true},
        new object[] { "001", "12345678000942", true},
        new object[] { "001", "12345678900", false},
        new object[] { "001", "12345678000900", false},
        new object[] { "001", "123456789123", false},
        new object[] { "abc", "12345678909", false}
    };

    [Theory, MemberData(nameof(TheoryData))]

    public void ValidateCreateAccountRequest_GivenARequest_MusBeValidatedLikeExpected(string agency, string document, bool expected)
    {
        var mockRequest = new CreateAccountRequest(agency, document);

        var result = _validator.Validate(mockRequest);

        result.IsValid.Should().Be(expected);
    }
}