using AccountingService.Domain.CommandHandlers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using AccountingService.Repository.Contexts;
using AccountingService.Repository.Repositories;
using AutoFixture;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccountingService.UnitTests.Domain.CommandHandlers;

public class DeactivateAccountCommandHandlerTests
{
    // Given
    
    private readonly Fixture _fixture = new();
    private readonly Mock<IAccountRepository> _mockRepository = new();
    private readonly IAccountRepository _repository;
    private readonly Mock<IMediator> _mediator = new();
    private readonly DeactivateAccountCommandHandler _mockHandler;
    private readonly DeactivateAccountCommandHandler _handler;

    private readonly Account _account;

    public DeactivateAccountCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ReadModelSqlContext>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;
        var readModelSqlContext = new ReadModelSqlContext(options);
        _repository = new AccountSqlRepository(readModelSqlContext);

        _mockHandler = new(_mediator.Object, _mockRepository.Object);
        _handler = new(_mediator.Object, _repository);

        _account = _fixture.Build<Account>().Create();
    }


    [Fact]
    public async void Handle_GivenAValidEvent_ShouldCreateAccountEventAndSaveChanges()
    {
        // When
        var @event = _fixture.Create<DeactivateAccountCommand>();
        await _mockHandler.Handle(@event, CancellationToken.None);
        
        // Then
        _mockRepository.Verify(x => x.Save(), Times.Once);
        
    }

    // Given
    public static readonly object[][] TheoryData =
    {
        new object[] { DateTime.Now.AddDays(-1), 0, 1},
        new object[] { DateTime.Now.AddDays(1), 0, 0},
        new object[] { DateTime.Now.AddDays(-1), 1, 0},
        new object[] { DateTime.Now.AddDays(-1), -1, 0},
    };
    [Theory, MemberData(nameof(TheoryData))]
    public async void DeactivateAccountCommandHandler_GivenRequest_ShouldReturnExpected(DateTime openingDate, decimal balance, int expected)
    {
        
        // Given
        _account.OpeningDate = openingDate;
        _account.Balance = balance;
        await _repository.AddAccount(_account);
        await _repository.Save();
        var command = new DeactivateAccountCommand(){Id = _account.Id};
  
        // When
        await _handler.Handle(command, CancellationToken.None);
        
        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<DeactivatedAccountEvent>(), 
            It.IsAny<CancellationToken>()), Times.Exactly(expected));
    }
    
    [Fact]
    public async void DeactivateAccountCommandHandlerTest()
    {
        // Given
        _account.OpeningDate = DateTime.Now.AddDays(-1);
        _account.Balance = 0;
        await _repository.AddAccount(_account);
        await _repository.Save();
        var command = new DeactivateAccountCommand(){Id = _account.Id};
        
        // When
        await _handler.Handle(command, CancellationToken.None);
        
        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<DeactivatedAccountEvent>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
}