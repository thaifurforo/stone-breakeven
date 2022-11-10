using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using MediatR;
using Newtonsoft.Json;

namespace AccountingService.Domain.EventHandlers;

public class DeactivatedAccountEventHandler : INotificationHandler<DeactivatedAccountEvent>
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public DeactivatedAccountEventHandler(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }

    public async Task Handle(DeactivatedAccountEvent notification, CancellationToken cancellationToken)
    {

        string jsonCreatedAccountMetadata = JsonConvert.SerializeObject(notification);
        var eventStore = new EventStore(nameof(DeactivatedAccountEvent), jsonCreatedAccountMetadata);
        
        await _eventStoreRepository.AddEvent(eventStore);
        await _eventStoreRepository.Save();
    }
}