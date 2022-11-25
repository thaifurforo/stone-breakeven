global using Xunit;
using AccountManagementService.Domain.Models;
using AutoFixture;

namespace AccountManagementService.UnitTests.Domain.Models;

public class AccountTests
{
    private static readonly Fixture _fixture = new();
    
    private readonly DateTime _genericCurrentDate = DateTime.Now;

    private const string ValidDocument = "12345678909";
    private const string ValidAgency = "001";
    private const decimal InitialBalance = 0;

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
         
         Assert.Equal(_genericCurrentDate.ToShortDateString(), account.OpeningDate.ToShortDateString());
     }

     [Fact]
     public void CreateAccount_ShouldClosingDateBeNull()
     {
         var account = new Account(ValidDocument, ValidAgency);

         Assert.Null(account.ClosingDate);
     }

     [Fact]
     public void CreateAccount_ShouldIsActiveBeTrue()
     {
         var account = new Account(ValidDocument, ValidAgency);

         Assert.True(account.IsActive);
     }

     [Fact]
     public void CreateAccount_ShouldBalanceBeZero()
     {
         var account = new Account(ValidDocument, ValidAgency);
         
         Assert.Equal(InitialBalance, account.Balance);
     }
     
}