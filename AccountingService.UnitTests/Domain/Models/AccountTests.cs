global using Xunit;
using AccountingService.Domain.Models;
using AutoFixture;

namespace AccountingService.UnitTests.Domain.Models;

public class AccountTests
{
    private static readonly Fixture _fixture = new();
    
    public AccountTests()
    {
        _fixture.Register(() => default(DateOnly));
    }
    
    private readonly DateOnly _genericCurrentDate = DateOnly.FromDateTime(DateTime.Today);

    private const string ValidDocument = "12345678909";
    private const string ValidAgency = "001";
    private const decimal InitialAmount = 0;

    [Fact]
    public void CreateAccount_GivenDocument_ShouldAccountDocumentBeCorrectlySaved()
    {
        var account = _fixture.Build<Account>()
            .With(x => x.Document, ValidDocument)
            .Create();
        
        Assert.Equal(ValidDocument, account.Document);
    }

     [Fact]
     public void CreateAccount_GivenAgency_ShouldAccountAgencyBeCorrectlySaved()
     {
         
         var account = _fixture.Build<Account>()
             .With(x => x.Agency, ValidAgency)
             .Create();

         Assert.Equal(ValidAgency, account.Agency);
     }

     [Fact]
     public void CreateAccount_ShouldOpeningDateBeCurrentDate()
     {
         var account = new Account(ValidDocument, ValidAgency);
         
         Assert.Equal(_genericCurrentDate, account.OpeningDate);
     }

     [Fact]
     public void CreateAccount_ShouldClosingDateBeNull()
     {
         var account = new Account(ValidDocument, ValidAgency);

         Assert.Null(account.ClosingDate);
     }

     [Fact]
     public void CreateAccount_ShouldStatusBeTrue()
     {
         var account = new Account(ValidDocument, ValidAgency);

         Assert.True(account.Status);
     }

     [Fact]
     public void CreateAccount_ShouldAmountBeZero()
     {
         var account = new Account(ValidDocument, ValidAgency);
         
         Assert.Equal(InitialAmount, account.Amount);
     }
     
}