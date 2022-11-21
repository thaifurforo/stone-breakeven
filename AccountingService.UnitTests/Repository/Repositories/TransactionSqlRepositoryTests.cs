using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AccountingService.UnitTests.Repository.Repositories;

public class TransactionSqlRepositoryTests
{
    private readonly Mock<ITransactionRepository> _repository = new ();
    private readonly Fixture _fixture = new();
    private readonly Transaction _transaction;
    private readonly List<Transaction> _transactions = new();
    private const int GenericInt = 1;

    public TransactionSqlRepositoryTests()
    {
        _transaction = _fixture.Build<Transaction>().Create();
        _transactions.Add(_transaction);
    }

    [Fact]
    public void GetTransactionById_GivenId_ShouldReturnTransaction()
    {
        // Given
        _repository.Setup(x => x.GetTransactionById(It.IsAny<Guid>())).ReturnsAsync(_transaction);
        
        // When
        var result = _repository.Object.GetTransactionById(_transaction.TransactionId).Result;
        
        // Then
        _repository.Verify(x => x.GetTransactionById(It.IsAny<Guid>()), Times.Once);
        result.Should().BeOfType(typeof(Transaction));
        result.Should().Be(_transaction);
    }
    
    [Fact]
    public void GetAllTransactions_ShouldReturnListOfTransactions()
    {
        // Given
        _repository.Setup(x => x.GetAllTransactions()).ReturnsAsync(_transactions);
        
        // When
        var result = _repository.Object.GetAllTransactions().Result;
        
        // Then
        _repository.Verify(x => x.GetAllTransactions(), Times.Once);
        result.Should().BeOfType(typeof(List<Transaction>));
        result.Should().Equal(_transactions);
    }
    
    [Fact]
    public void AddTransaction_GivenTransaction_ShouldReturnTransaction()
    {
        // Given
        _repository.Setup(x => x.AddTransaction(It.IsAny<Transaction>())).ReturnsAsync(_transaction);
        
        // When
        var result = _repository.Object.AddTransaction(_transaction).Result;
        
        // Then
        _repository.Verify(x => x.AddTransaction(It.IsAny<Transaction>()), Times.Once);
        result.Should().BeOfType(typeof(Transaction));
        result.Should().Be(_transaction);
    }
    
    [Fact]
    public void GetTransactionsByAccountId_GivenAccountId_ShouldReturnTransactions()
    {
        // Given
        _repository.Setup(x => x.GetTransactionsByAccountId(It.IsAny<int>())).ReturnsAsync(_transactions);
        
        // When
        var result = _repository.Object.GetTransactionsByAccountId(GenericInt).Result;
        
        // Then
        _repository.Verify(x => x.GetTransactionsByAccountId(It.IsAny<int>()), Times.Once);
        result.Should().BeOfType(typeof(List<Transaction>));
        result.Should().Equal(_transactions);
    }
    
    [Fact]
    public void GetCreditTransactionsByAccountId_GivenAccountId_ShouldReturnTransactions()
    {
        // Given
        _repository.Setup(x => x.GetCreditTransactionsByAccountId(It.IsAny<int>())).ReturnsAsync(_transactions);
        
        // When
        var result = _repository.Object.GetCreditTransactionsByAccountId(GenericInt).Result;
        
        // Then
        _repository.Verify(x => x.GetCreditTransactionsByAccountId(It.IsAny<int>()), Times.Once);
        result.Should().BeOfType(typeof(List<Transaction>));
        result.Should().Equal(_transactions);
    }

    [Fact]
    public void GetDebitTransactionsByAccountId_GivenAccountId_ShouldReturnTransactions()
    {
        // Given
        _repository.Setup(x => x.GetDebitTransactionsByAccountId(It.IsAny<int>())).ReturnsAsync(_transactions);
        
        // When
        var result = _repository.Object.GetDebitTransactionsByAccountId(GenericInt).Result;
        
        // Then
        _repository.Verify(x => x.GetDebitTransactionsByAccountId(It.IsAny<int>()), Times.Once);
        result.Should().BeOfType(typeof(List<Transaction>));
        result.Should().Equal(_transactions);
    }

    [Fact]
    public void SaveTest()
    {
       // When
        var result = _repository.Object.Save();
        
        // Then
        _repository.Verify(x => x.Save(), Times.Once);
        result.IsCompletedSuccessfully.Should().BeTrue();
    }

}