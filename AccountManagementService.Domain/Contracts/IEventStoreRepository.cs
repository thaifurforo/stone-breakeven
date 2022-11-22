using AccountManagementService.Domain.Models;

namespace AccountManagementService.Domain.Contracts
{

    public interface IEventStoreRepository : IDisposable
    {

        Task<IEnumerable<EventStore>> GetAllEvents();
        Task<EventStore?> GetEventById(Guid id);
        Task<IEnumerable<EventStore?>> GetEventsByAccountId(int id);
        Task<IEnumerable<EventStore?>> GetEventsByName(string eventName);
        Task<EventStore> AddEvent(EventStore eventStore);
        Task Save();
    }
}