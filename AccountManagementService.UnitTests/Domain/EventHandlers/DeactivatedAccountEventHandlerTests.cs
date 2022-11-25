using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.EventHandlers;
using AccountManagementService.Domain.Models;
using AccountManagementService.Domain.Notifications;
using AutoFixture;
using Moq;

namespace AccountManagementService.UnitTests.Domain.EventHandlers;

public class DeactivatedAccountEventHandlerTests
{
    // Given
    private readonly Mock<IEventStoreRepository> _repository = new();
    private readonly Fixture _fixture = new();
    private readonly DeactivatedAccountEventHandler _handler;

    public DeactivatedAccountEventHandlerTests()
    {
        _handler = new(_repository.Object);
    }

    [Fact]
    public async Task Handle_GivenAValidEvent_ShouldSaveEvent()
    {
        // Given
        var @event = _fixture.Create<DeactivatedAccountEvent>();

        // When
        await _handler.Handle(@event, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.AddEvent(It.IsAny<EventStore>()), Times.Once);
    }
}
