using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Models;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AccountManagementService.UnitTests.Repository.Repositories;

public class AccountSqlRepositoryTests
{
    private readonly Mock<IAccountRepository> _repository = new ();
    private readonly Fixture _fixture = new();
    private readonly Account _account;
    private readonly List<Account> _accounts = new();

    public AccountSqlRepositoryTests()
    {
        _account = _fixture.Build<Account>().Create();
        _accounts.Add(_account);
    }

    [Fact]
    public void GetAccountById_GivenId_ShouldReturnAccount()
    {
        // Given
        _repository.Setup(x => x.GetAccountById(It.IsAny<int>())).ReturnsAsync(_account);
        
        // When
        var result = _repository.Object.GetAccountById(_account.Id).Result;
        
        // Then
        _repository.Verify(x => x.GetAccountById(It.IsAny<int>()), Times.Once);
        result.Should().BeOfType(typeof(Account));
        result.Should().Be(_account);
    }
    
    [Fact]
    public void GetAllAccounts_ShouldReturnListOfAccounts()
    {
        // Given
        _repository.Setup(x => x.GetAllAccounts()).ReturnsAsync(_accounts);
        
        // When
        var result = _repository.Object.GetAllAccounts().Result;
        
        // Then
        _repository.Verify(x => x.GetAllAccounts(), Times.Once);
        result.Should().BeOfType(typeof(List<Account>));
        result.Should().Equal(_accounts);
    }
    
    [Fact]
    public void AddAccount_GivenAccount_ShouldReturnAccount()
    {
        // Given
        _repository.Setup(x => x.AddAccount(It.IsAny<Account>())).ReturnsAsync(_account);
        
        // When
        var result = _repository.Object.AddAccount(_account).Result;
        
        // Then
        _repository.Verify(x => x.AddAccount(It.IsAny<Account>()), Times.Once);
        result.Should().BeOfType(typeof(Account));
        result.Should().Be(_account);
    }
    
    [Fact]
    public void UpdateAccount_GivenAccount_ShouldReturnAccount()
    {
        // Given
        _repository.Setup(x => x.UpdateAccount(It.IsAny<Account>())).ReturnsAsync(_account);
        
        // When
        var result = _repository.Object.UpdateAccount(_account).Result;
        
        // Then
        _repository.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        result.Should().BeOfType(typeof(Account));
        result.Should().Be(_account);
    }

    [Fact]
    public void DeactivateAccount_GivenId_ShouldReturnAccount()
    {
        // Given
        _repository.Setup(x => x.DeactivateAccount(It.IsAny<int>())).ReturnsAsync(_account);
        
        // When
        var result = _repository.Object.DeactivateAccount(_account.Id).Result;
        
        // Then
        _repository.Verify(x => x.DeactivateAccount(It.IsAny<int>()), Times.Once);
        result.Should().BeOfType(typeof(Account));
        result.Should().Be(_account);
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