using AccountingService.Domain.Contracts;
using AccountingService.Domain.EventHandlers;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using AutoFixture;
using Moq;

namespace AccountingService.UnitTests.Domain.EventHandlers;

public class CreatedTransactionEventHandlerTests
{
    // Given
    private readonly Mock<IEventStoreRepository> _repository = new();
    private readonly Fixture _fixture = new();
    private readonly CreatedTransactionEventHandler _handler;

    public CreatedTransactionEventHandlerTests()
    {
        _handler = new(_repository.Object);
    }

    [Fact]
    public async Task Handle_GivenAValidEvent_ShouldSaveEvent()
    {
        // Given
        var @event = _fixture.Create<CreatedTransactionEvent>();

        // When
        await _handler.Handle(@event, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.AddEvent(It.IsAny<EventStore>()), Times.Once);
    }
}