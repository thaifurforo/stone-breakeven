using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Contracts.v1.Response;
using AccountingService.Api.Contracts.v1.Validators;
using AccountingService.Domain.Models;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace AccountingService.UnitTests.Api.Contracts.v1.Validators;

public class DeactivateAccountRequestValidatorTests
{
    private readonly DeactivateAccountRequestValidator _validator = new();
    private readonly Fixture _fixture = new();
    
    public DeactivateAccountRequestValidatorTests()
    {
        _fixture.Register(() => default(DateOnly));
    }
    
    public static readonly object[][] TheoryData =
    {
        new object[] { new DateOnly(2022, 11, 4), new DateOnly(2022, 11, 5), 0, true},
        new object[] { new DateOnly(2022, 11, 4), new DateOnly(2022, 11, 3), 0, false},
        new object[] { new DateOnly(2022, 11, 4), new DateOnly(2022, 11, 5), 1, false},
        new object[] { new DateOnly(2022, 11, 4), new DateOnly(2022, 11, 5), -1, false},
    };

    [Theory, MemberData(nameof(TheoryData))]

    public void ValidateDeactivateAccountRequest_GivenARequest_MustBeValidatedLikeExpected(DateOnly openingDate,
        DateOnly closingDate, decimal balance, bool expected)
    {
        Account account = _fixture.Build<Account>()
            .Create();
        
        AccountList.Accounts.Add(account);
        
        var mockRequest = new DeactivateAccountRequest(account.Id) {OpeningDate = openingDate, ClosingDate = closingDate, Balance = balance};

        var result = _validator.Validate(mockRequest);

        result.IsValid.Should().Be(expected);
    }
}