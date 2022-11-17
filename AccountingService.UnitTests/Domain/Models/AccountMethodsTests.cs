using AccountingService.Domain.Models;
using AutoFixture;
using FluentAssertions;

namespace AccountingService.UnitTests.Domain.Models;

public class AccountMethodsTests
{
    
    private static readonly Fixture _fixture = new();
    private static readonly DateTime? NullDateTime = null;

    [Fact]
    public void CreateAccount_GivenAnyIdNumber_ShouldAccountNumberBeCorrectlyGenerated()
    {
        var account = _fixture.Build<Account>()
            .Create();

        string accountNumber = account.GetAccountNumber();

        Assert.Equal(accountNumber, account.Number);
    }

    [Theory]
    [InlineData(1, "000001-2")]
    [InlineData(24, "000024-3")]
    [InlineData(897, "000897-7")]
    [InlineData(3674, "003674-2")]
    [InlineData(98741, "098741-4")]
    [InlineData(542198, "542198-6")]
    public void CreateAccount_GivenAnIdNumber_ShouldAccountNumberBeCorrectlyGenerated(int id, string expectedAccountNumber)
    {
        
        var account = _fixture.Build<Account>()
            .With(x => x.Id, id)
            .Create();

        account.Number = account.GetAccountNumber();
        
        account.Number.Should().Be(expectedAccountNumber);
    }
    
    public static readonly object[][] TheoryData =
    {
        new object[] { DateTime.Now.AddDays(-1), true, 0, true},
        new object[] { DateTime.Now.AddDays(1), true, 0, false},
        new object[] { DateTime.Now.AddDays(-1), false, 0, false},
        new object[] { DateTime.Now.AddDays(-1), true, 10, false},
    };
    [Theory, MemberData(nameof(TheoryData))]
    public void DeactivateAccount_GivenAccount_ShouldReturnExpected(DateTime openingDate, bool isActive, decimal balance, bool expected)
    {
        var account = _fixture.Build<Account>()
            .With(x => x.OpeningDate, openingDate)
            .With(x => x.IsActive, isActive)
            .With(x => x.Balance, balance)
            .With(x => x.ClosingDate, NullDateTime)
            .Create();

        var result = true;
        
        try
        {
            account.DeactivateAccount();
        }
        catch (Exception ex)
        {
            result = false;
        }
        
        Assert.Equal(expected, result);
    }
    
}