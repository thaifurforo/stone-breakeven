using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;

namespace AccountingService.Repository.Repositories
{
    public class EventStoreSqlRepository : IEventStoreRepository, IDisposable
    {
        private readonly EventStoreSqlContext _eventStoreSqlContext;
        
        public EventStoreSqlRepository(EventStoreSqlContext eventStoreSqlContext)
        {
            _eventStoreSqlContext = eventStoreSqlContext ?? throw new ArgumentNullException(nameof(eventStoreSqlContext));
        }

        public async Task<IEnumerable<EventStore>> GetAllEvents()
        {
            return await Task.Run(() => _eventStoreSqlContext.AccountEventsMetadata.ToList());
        }

        public async Task<EventStore?> GetEventById(int id)
        {
            return await Task.Run(() => _eventStoreSqlContext.AccountEventsMetadata.Find(id));
        }
        
        public async Task<EventStore> AddEvent(EventStore eventStore)
        {
            return await Task.Run(() => _eventStoreSqlContext.AccountEventsMetadata.Add(eventStore).Entity);
        }
        
        public async Task Save()
        {
            await Task.Run(() => _eventStoreSqlContext.SaveChanges(true));
        }

        public bool disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _eventStoreSqlContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
    }
}