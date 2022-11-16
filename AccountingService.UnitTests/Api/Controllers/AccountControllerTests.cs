using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Controllers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Repository;
using AccountingService.Repository.Repositories;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccountingService.UnitTests.Api.Controllers;

public class AccountControllerTests : IAsyncLifetime
{
    private readonly AccountController _accountController;
    private readonly IAccountRepository _accountRepository;
    private readonly Mock<IMediator> _mediator = new();

    private readonly Account _account;
    
    private const string GenericAgency = "001";
    private const string GenericDocument = "12345678909";
    

    public AccountControllerTests()
    {
        var options = new DbContextOptionsBuilder<ReadModelSqlContext>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;
        var readModelSqlContext = new ReadModelSqlContext(options);
        _accountRepository = new AccountSqlRepository(readModelSqlContext);
        _accountController = new AccountController(_accountRepository, _mediator.Object);
        
        _account = new Account(GenericDocument, GenericAgency);
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
        await _accountController.CreateAccount(It.IsAny<CreateAccountCommand>());
        _mediator.Verify(x => x.Send(It.IsAny<CreateAccountCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void DeactivateAccountTest()
    {
        var result = await _accountController.DeactivateAccount(new GetByAccountId() { Id = _account.Id });

        _mediator.Verify(x => x.Send(It.IsAny<DeactivateAccountCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetAllAccountsTest()
    {
        var result = await _accountController.GetAllAccounts();
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetAccountTest()
    {
        var request = new GetByAccountId() { Id = _account.Id };
        var result = await _accountController.GetAccount(request);
        result.Should().BeOfType<OkObjectResult>();
    }
}
