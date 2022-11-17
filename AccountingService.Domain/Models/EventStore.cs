namespace AccountingService.Domain.Models;

public class EventStore
{
    public Guid EventStoreId { get; set; }
    public string EventName { get; set; }
    public string Metadata { get; set; }

    public EventStore(string eventName, string metadata)
    {
        EventName = eventName;
        Metadata = metadata;
    }

    public EventStore()
    {
    }
}