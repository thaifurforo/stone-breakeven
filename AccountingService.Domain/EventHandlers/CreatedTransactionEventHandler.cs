using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using MediatR;
using Newtonsoft.Json;

namespace AccountingService.Domain.EventHandlers;

public class CreatedTransactionEventHandler : INotificationHandler<CreatedTransactionEvent>
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public CreatedTransactionEventHandler(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }

    public async Task Handle(CreatedTransactionEvent notification, CancellationToken cancellationToken)
    {

        string jsonCreatedAccountMetadata = JsonConvert.SerializeObject(notification);
        var eventStore = new EventStore(nameof(CreatedTransactionEvent), jsonCreatedAccountMetadata);
        
        await _eventStoreRepository.AddEvent(eventStore);
        await _eventStoreRepository.Save();
    }
}