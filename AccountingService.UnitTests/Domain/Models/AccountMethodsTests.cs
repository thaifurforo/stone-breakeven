using AccountingService.Domain.Models;
using AutoFixture;

namespace AccountingService.UnitTests.Domain.Models;

public class AccountMethodsTests
{
    
    private static readonly Fixture _fixture = new();
    
    public AccountMethodsTests()
    {
        _fixture.Register(() => default(DateOnly));
    }
    
    [Fact]
    public void FindAccountById_GivenAnIdNumber_ShouldAccountNumberBeCorrectlyGenerated()
    {
        var account = _fixture.Build<Account>()
            .Create();

        string accountNumber = account.GetAccountNumber(account.Id);

        Assert.Equal(accountNumber, account.Number);
    }
}