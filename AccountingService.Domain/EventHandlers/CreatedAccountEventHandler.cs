using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using MediatR;
using Newtonsoft.Json;

namespace AccountingService.Domain.EventHandlers;

public class CreatedAccountEventHandler : INotificationHandler<CreatedAccountEvent>
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public CreatedAccountEventHandler(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }

    public async Task Handle(CreatedAccountEvent notification, CancellationToken cancellationToken)
    {

        string jsonCreatedAccountMetadata = JsonConvert.SerializeObject(notification);
        var eventStore = new EventStore(nameof(CreatedAccountEvent), jsonCreatedAccountMetadata);
        
        await _eventStoreRepository.AddEvent(eventStore);
        await _eventStoreRepository.Save();
    }
}