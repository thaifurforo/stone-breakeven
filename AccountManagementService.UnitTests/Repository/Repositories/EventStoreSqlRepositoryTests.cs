using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Models;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AccountManagementService.UnitTests.Repository.Repositories;

public class EventStoreSqlRepositoryTests
{
    private readonly Mock<IEventStoreRepository> _repository = new ();
    private readonly Fixture _fixture = new();
    private readonly EventStore _event;
    private readonly List<EventStore> _events = new();
    private const int GenericInt = 1;
    private const string GenericString = "xpto";

    public EventStoreSqlRepositoryTests()
    {
        _event = _fixture.Build<EventStore>().Create();
        _events.Add(_event);
    }

    [Fact]
    public void GetEventById_GivenId_ShouldReturnEventStore()
    {
        // Given
        _repository.Setup(x => x.GetEventById(It.IsAny<Guid>())).ReturnsAsync(_event);
        
        // When
        var result = _repository.Object.GetEventById(_event.EventStoreId).Result;
        
        // Then
        _repository.Verify(x => x.GetEventById(It.IsAny<Guid>()), Times.Once);
        result.Should().BeOfType(typeof(EventStore));
        result.Should().Be(_event);
    }
    
    [Fact]
    public void GetAllEvents_ShouldReturnListOfEventStores()
    {
        // Given
        _repository.Setup(x => x.GetAllEvents()).ReturnsAsync(_events);
        
        // When
        var result = _repository.Object.GetAllEvents().Result;
        
        // Then
        _repository.Verify(x => x.GetAllEvents(), Times.Once);
        result.Should().BeOfType(typeof(List<EventStore>));
        result.Should().Equal(_events);
    }
    
    [Fact]
    public void AddEvent_GivenEvent_ShouldReturnEventStore()
    {
        // Given
        _repository.Setup(x => x.AddEvent(It.IsAny<EventStore>())).ReturnsAsync(_event);
        
        // When
        var result = _repository.Object.AddEvent(_event).Result;
        
        // Then
        _repository.Verify(x => x.AddEvent(It.IsAny<EventStore>()), Times.Once);
        result.Should().BeOfType(typeof(EventStore));
        result.Should().Be(_event);
    }
    
    [Fact]
    public void GetEventsByAccountId_GivenAccountId_ShouldReturnEventStores()
    {
        // Given
        _repository.Setup(x => x.GetEventsByAccountId(It.IsAny<int>())).ReturnsAsync(_events);
        
        // When
        var result = _repository.Object.GetEventsByAccountId(GenericInt).Result;
        
        // Then
        _repository.Verify(x => x.GetEventsByAccountId(It.IsAny<int>()), Times.Once);
        result.Should().BeOfType(typeof(List<EventStore>));
        result.Should().Equal(_events);
    }
    
    [Fact]
    public void GetEventsByName_GivenName_ShouldReturnEventStores()
    {
        // Given
        _repository.Setup(x => x.GetEventsByName(It.IsAny<string>())).ReturnsAsync(_events);
        
        // When
        var result = _repository.Object.GetEventsByName(GenericString).Result;
        
        // Then
        _repository.Verify(x => x.GetEventsByName(It.IsAny<string>()), Times.Once);
        result.Should().BeOfType(typeof(List<EventStore>));
        result.Should().Equal(_events);
    }

    [Fact]
    public void SaveTest()
    {
       // When
        var result = _repository.Object.Save();
        
        // Then
        _repository.Verify(x => x.Save(), Times.Once);
        result.IsCompletedSuccessfully.Should().BeTrue();
    }

}