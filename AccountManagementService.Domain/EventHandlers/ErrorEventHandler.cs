using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Models;
using AccountManagementService.Domain.Notifications;
using MediatR;
using Newtonsoft.Json;

namespace AccountManagementService.Domain.EventHandlers;

public class ErrorEventHandler : INotificationHandler<ErrorEvent>
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public ErrorEventHandler(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }

    public async Task Handle(ErrorEvent notification, CancellationToken cancellationToken)
    {

        string jsonCreatedAccountMetadata = JsonConvert.SerializeObject(notification);
        var eventStore = new EventStore(nameof(ErrorEvent), jsonCreatedAccountMetadata);
        
        await _eventStoreRepository.AddEvent(eventStore);
        await _eventStoreRepository.Save();
    }
}