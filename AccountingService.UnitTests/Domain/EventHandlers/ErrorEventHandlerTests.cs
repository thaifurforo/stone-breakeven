using AccountingService.Domain.Contracts;
using AccountingService.Domain.EventHandlers;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using AutoFixture;
using Moq;
using ErrorEventHandler = AccountingService.Domain.EventHandlers.ErrorEventHandler;

namespace AccountingService.UnitTests.Domain.EventHandlers;

public class ErrorEventHandlerTests
{
    // Given
    private readonly Mock<IEventStoreRepository> _repository = new();
    private readonly Fixture _fixture = new();
    private readonly ErrorEventHandler _handler;

    public ErrorEventHandlerTests()
    {
        _handler = new(_repository.Object);
    }

    [Fact]
    public async Task Handle_GivenAValidEvent_ShouldSaveEvent()
    {
        // Given
        var @event = _fixture.Create<ErrorEvent>();

        // When
        await _handler.Handle(@event, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.AddEvent(It.IsAny<EventStore>()), Times.Once);
    }
}
