using AccountingService.Api.Controllers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Repository;
using AccountingService.Repository.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccountingService.UnitTests.Api.Controllers;

public class AccountControllerTests
{
    private readonly AccountController _accountController;
    private readonly ReadModelInMemoryContext _readModelInMemoryContext;
    private readonly IAccountRepository _accountRepository;
    private readonly Mock<IMediator> _mediator = new();


    public AccountControllerTests()
    {
        var options = new DbContextOptionsBuilder<ReadModelInMemoryContext>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;
        _readModelInMemoryContext = new ReadModelInMemoryContext(options);
        _accountRepository = new AccountInMemoryRepository(_readModelInMemoryContext);

        _accountController = new AccountController(_accountRepository, _mediator.Object);
    }

    [Fact]
    public async void CreateAccountTest()
    {
        // _mediator.Setup(x => x.Send(It.IsAny<IRequest<Unit>>(), It.IsAny<CancellationToken>()));
        await _accountController.CreateAccount(It.IsAny<CreateAccountCommand>());
        _mediator.Verify(x => x.Send(It.IsAny<CreateAccountCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async void DeactivateAccountTest()
    {
        await _accountController.DeactivateAccount(It.IsAny<int>());
        _mediator.Verify(x => x.Send(It.IsAny<DeactivateAccountCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
}

