using AccountingService.Domain.Models;

namespace AccountingService.Domain.Contracts
{

    public interface IEventStoreRepository : IDisposable
    {

        Task<IEnumerable<EventStore>> GetAllEvents();
        Task<EventStore?> GetEventById(int id);
        Task<EventStore> AddEvent(EventStore eventStore);
        Task Save();
    }
}