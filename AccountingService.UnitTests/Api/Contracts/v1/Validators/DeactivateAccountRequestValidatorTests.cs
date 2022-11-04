using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Contracts.v1.Validators;
using FluentAssertions;

namespace AccountingService.UnitTests.Api.Contracts.v1.Validators;

public class DeactivateAccountRequestValidatorTests
{
    private readonly DeactivateAccountRequestValidator _validator = new();
    
    public static readonly object[][] TheoryData =
    {
        new object[] { new DateOnly(2022, 11, 4), new DateOnly(2022, 11, 5), true},
        new object[] { new DateOnly(2022, 11, 4), new DateOnly(2022, 11, 3), false}
    };

    [Theory, MemberData(nameof(TheoryData))]

    public void ValidateDeactivateAccountRequest_GivenARequest_MusBeValidatedLikeExpected(DateOnly openingDate,
        DateOnly closingDate, bool expected)
    {
        var mockRequest = new DeactivateAccountRequest() {OpeningDate = openingDate, ClosingDate = closingDate};

        var result = _validator.Validate(mockRequest);

        result.IsValid.Should().Be(expected);
    }
}