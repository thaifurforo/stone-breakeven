global using Xunit;
using AccountManagementService.Domain.Models;
using AutoFixture;

namespace AccountManagementService.UnitTests.Domain.Models;

public class TransactionTests
{
    private readonly Fixture _fixture = new();

    private const string GenericTransactionType = "deposit";
    private const int GenericAccount = 1;
    private const decimal GenericAmount = 10.5m;
    
    [Fact]
    public void CreateTransaction_GivenTransactionType_ShouldTransactionTypeBeCorrectlySaved()
    {
        var account = _fixture.Build<Transaction>()
            .With(x => x.TransactionType, GenericTransactionType)
            .Create();
        
        Assert.Equal(GenericTransactionType, account.TransactionType);
    }

     [Fact]
     public void CreateTransaction_GivenDebitAccount_ShouldDebitAccountBeCorrectlySaved()
     {
         
         var account = _fixture.Build<Transaction>()
             .With(x => x.DebitAccountId, GenericAccount)
             .Create();

         Assert.Equal(GenericAccount, account.DebitAccountId);
     }

     [Fact]
     public void CreateTransaction_GivenCreditAccount_ShouldDebitAccountBeCorrectlySaved()
     {
         
         var account = _fixture.Build<Transaction>()
             .With(x => x.CreditAccountId, GenericAccount)
             .Create();

         Assert.Equal(GenericAccount, account.CreditAccountId);
     }

     [Fact]
     public void CreateTransaction_GivenAmount_ShouldAmountBeCorrectlySaved()
     {
         
         var account = _fixture.Build<Transaction>()
             .With(x => x.Amount, GenericAmount)
             .Create();

         Assert.Equal(GenericAmount, account.Amount);
     }
     
     [Fact]
     public void CreateTransaction_ShouldTransactionDateBeNow()
     {
         var transaction = new Transaction(GenericTransactionType, GenericAmount, GenericAccount);

         Assert.Equal(DateTime.Now.Date, transaction.TransactionDate.Date);
     }
}