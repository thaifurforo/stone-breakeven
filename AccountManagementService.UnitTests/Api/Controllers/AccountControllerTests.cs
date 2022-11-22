using AccountManagementService.Api.Contracts.v1.Requests;
using AccountManagementService.Api.Controllers;
using AccountManagementService.Domain.Commands;
using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Models;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AccountManagementService.UnitTests.Api.Controllers;

public class AccountControllerTests
{
    // Given
    
    private readonly AccountController _accountController;
    private readonly Mock<IAccountRepository> _accountRepository = new();
    private readonly Mock<IMediator> _mediator = new();
    private readonly Account _account;
    private readonly List<Account> _accounts = new();
    private readonly GetByAccountId _getByAccountIdRequest;
    private readonly CreateAccountCommand _createAccountCommand;

    public AccountControllerTests()
    {
        _accountController = new AccountController(_accountRepository.Object, _mediator.Object);

        var fixture = new Fixture();
        _account = fixture.Build<Account>().Create();
        _getByAccountIdRequest = fixture.Build<GetByAccountId>().Create();
        _createAccountCommand = fixture.Build<CreateAccountCommand>().Create();
        _accounts.Add(_account);
    }
    
    [Fact]
    public async void CreateAccount_GivenCommand_ShouldReturnOk()
    {
        // When
        var result = await _accountController.CreateAccount(_createAccountCommand);
        
        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void CreateAccount_GivenCommand_ShouldSendCommand()
    {
        // When
        await _accountController.CreateAccount(_createAccountCommand);
        
        // Then
        _mediator.Verify(x => x.Send(It.IsAny<CreateAccountCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
 [Fact]
    public async void DeactivateAccount_GivenRequest_ShouldReturnOk()
    {
        // When
        var result = await _accountController.DeactivateAccount(_getByAccountIdRequest);
        
        // Then        
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void DeactivateAccount_GivenRequest_ShouldSendCommand()
    {
        // When
        await _accountController.DeactivateAccount(_getByAccountIdRequest);
        
        // Then        
        _mediator.Verify(x => x.Send(It.IsAny<DeactivateAccountCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async void GetAllAccounts_ShouldReturnOk()
    {
        _accountRepository.Setup(x => x.GetAllAccounts()).ReturnsAsync(_accounts);

        // When
        var result = await _accountController.GetAllAccounts();

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetAllAccounts_ShouldGetFromRepository()
    {
       // When
        await _accountController.GetAllAccounts();
        
        // Then
        _accountRepository.Verify(x => x.GetAllAccounts(), Times.Once);
    }
    
    [Fact]
    public async void GetAccountById_GivenValidRequest_ShouldReturnOk()
    {
        _accountRepository.Setup(x => x.GetAccountById(It.IsAny<int>())).ReturnsAsync(_account);
        
        // When
        var result = await _accountController.GetAccount(_getByAccountIdRequest);
        
        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetAccountById_GivenValidRequest_ShouldGetFromRepository()
    {
        // When
        await _accountController.GetAccount(_getByAccountIdRequest);
        
        // Then
        _accountRepository.Verify(x => x.GetAccountById(It.IsAny<int>()), Times.Once);
    }
}
