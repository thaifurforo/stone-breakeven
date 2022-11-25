using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Models;
using AccountManagementService.Domain.Notifications;
using AutoFixture;
using Moq;
using ErrorEventHandler = AccountManagementService.Domain.EventHandlers.ErrorEventHandler;

namespace AccountManagementService.UnitTests.Domain.EventHandlers;

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
