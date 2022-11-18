using AccountingService.Domain.CommandHandlers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using AccountingService.Repository.Contexts;
using AutoFixture;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccountingService.UnitTests.Domain.CommandHandlers;

public class DeactivateAccountCommandHandlerTests
{
    // Given
    
    private readonly Fixture _fixture = new();
    private readonly Mock<IAccountRepository> _repository = new();
    private readonly Mock<IMediator> _mediator = new();
    private readonly DeactivateAccountCommandHandler _handler;

    private readonly Account _account;

    public DeactivateAccountCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ReadModelSqlContext>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;
        var readModelSqlContext = new ReadModelSqlContext(options);

        _handler = new(_mediator.Object, _repository.Object);

        _account = _fixture.Build<Account>()
            .With(x => x.OpeningDate, DateTime.Now)
            .With(x => x.Balance, 0)
            .Create();
    }


    [Fact]
    public async void Handle_GivenAValidEvent_ShouldCreateAccountEventAndSaveChanges()
    {
        // When
        var @event = _fixture.Create<DeactivateAccountCommand>();
        await _handler.Handle(@event, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.Save(), Times.Once);
        
    }
    
    [Fact]
    public async void DeactivateAccountCommandHandlerTest()
    {
        // Given
        _repository.Setup(x => x.GetAccountById(It.IsAny<int>())).ReturnsAsync(_account);
        var command = new DeactivateAccountCommand(){Id = _account.Id};
        
        // When
        await _handler.Handle(command, CancellationToken.None);
        
        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<DeactivatedAccountEvent>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
}