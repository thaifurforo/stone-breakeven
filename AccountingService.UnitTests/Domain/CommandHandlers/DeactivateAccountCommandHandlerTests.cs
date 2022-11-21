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
    private readonly DeactivateAccountCommand _command;
    private readonly DeactivateAccountCommandHandler _handler;

    private readonly Account _account;

    public DeactivateAccountCommandHandlerTests()
    {
        _handler = new(_mediator.Object, _repository.Object);

        _account = _fixture.Build<Account>()
            .With(x => x.OpeningDate, DateTime.Now)
            .With(x => x.Balance, 0)
            .Create();

        _command = _fixture.Build<DeactivateAccountCommand>()
            .With(x => x.Id, _account.Id)
            .Create();
    }


    [Fact]
    public async void Handle_GivenAValidCommand_ShouldSave()
    {
        // When
        _repository.Setup(x => x.GetAccountById(It.IsAny<int>())).ReturnsAsync(_account);
        await _handler.Handle(_command, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.Save(), Times.Once);
    }
    
    [Fact]
    public async void Handle_GivenAValidCommand_ShouldMakeChangesToRepository()
    {
        // When
        _repository.Setup(x => x.GetAccountById(It.IsAny<int>())).ReturnsAsync(_account);
        await _handler.Handle(_command, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.DeactivateAccount(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async void DeactivateAccountCommandHandler_GivenValidCommand_ShouldPublishEvent()
    {
      
        // When
        _repository.Setup(x => x.GetAccountById(It.IsAny<int>())).ReturnsAsync(_account);
        await _handler.Handle(_command, CancellationToken.None);
        
        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<DeactivatedAccountEvent>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
}