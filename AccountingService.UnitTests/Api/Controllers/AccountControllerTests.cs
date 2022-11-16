using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Controllers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Repository;
using AccountingService.Repository.Repositories;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccountingService.UnitTests.Api.Controllers;

public class AccountControllerTests : IAsyncLifetime
{
    // Given
    
    private readonly AccountController _accountController;
    private readonly IAccountRepository _accountRepository;
    private readonly Mock<IMediator> _mediator = new();

    private readonly Account _account;

    public AccountControllerTests()
    {
        var options = new DbContextOptionsBuilder<ReadModelSqlContext>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;
        var readModelSqlContext = new ReadModelSqlContext(options);
        _accountRepository = new AccountSqlRepository(readModelSqlContext);
        _accountController = new AccountController(_accountRepository, _mediator.Object);

        var fixture = new Fixture();
        _account = fixture.Build<Account>().Create();
    }
    
    public async Task InitializeAsync()
    {
        await _accountRepository.AddAccount(_account);
        await _accountRepository.Save();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async void CreateAccountTest()
    {
        // _mediator.Setup(x => x.Send(It.IsAny<IRequest<Unit>>(), It.IsAny<CancellationToken>()));
        
        // When
        await _accountController.CreateAccount(It.IsAny<CreateAccountCommand>());
        
        // Then
        _mediator.Verify(x => x.Send(It.IsAny<CreateAccountCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void DeactivateAccountTest()
    {
        // When
        var result = await _accountController.DeactivateAccount(new GetByAccountId() { Id = _account.Id });
        
        // Then        
        _mediator.Verify(x => x.Send(It.IsAny<DeactivateAccountCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetAllAccountsTest()
    {
        // When
        var result = await _accountController.GetAllAccounts();
        
        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetAccountTest()
    {
        // When
        var request = new GetByAccountId() { Id = _account.Id };
        var result = await _accountController.GetAccount(request);
        
        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
}
