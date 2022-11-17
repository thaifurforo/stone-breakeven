global using Xunit;
using AccountingService.Domain.Models;
using AutoFixture;

namespace AccountingService.UnitTests.Domain.Models;

public class EventStoreTests
{
    private readonly Fixture _fixture = new();

    private const string GenericString = "xpto";

    [Fact]
    public void CreateEventStore_GivenMetadata_ShouldMetadataBeCorrectlySaved()
    {
        var eventStore = _fixture.Build<EventStore>()
            .With(x => x.Metadata, GenericString)
            .Create();
        
        Assert.Equal(GenericString, eventStore.Metadata);
    }
    
    [Fact]
    public void CreateEventStore_GivenEventName_ShouldEventNameBeCorrectlySaved()
    {
        var eventStore = _fixture.Build<EventStore>()
            .With(x => x.EventName, GenericString)
            .Create();
        
        Assert.Equal(GenericString, eventStore.EventName);
    }
}