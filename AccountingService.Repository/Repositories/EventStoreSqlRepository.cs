using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using Dasync.Collections;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository.Repositories
{
    public class EventStoreSqlRepository : IEventStoreRepository
    {
        private readonly EventStoreSqlContext _eventStoreSqlContext;
        
        public EventStoreSqlRepository(EventStoreSqlContext eventStoreSqlContext)
        {
            _eventStoreSqlContext = eventStoreSqlContext ?? throw new ArgumentNullException(nameof(eventStoreSqlContext));
        }

        public async Task<IEnumerable<EventStore>> GetAllEvents()
        {
            return await Task.Run(() => _eventStoreSqlContext.EventMetaData.ToList());
        }

        public async Task<EventStore?> GetEventById(Guid id)
        {
            return await await Task.Run(() => _eventStoreSqlContext.EventMetaData.FindAsync(id));
        }
        
        public async Task<IEnumerable<EventStore?>> GetEventsByAccountId(int id)
        {
            var query = _eventStoreSqlContext.EventMetaData
                .Where(x => x.Metadata.Contains($"\"Id\":{id}")
                            || x.Metadata.Contains($"AccountId\":{id}"));
            return await Task.Run(() => query.ToListAsync());
        }

        public async Task<IEnumerable<EventStore?>> GetEventsByName(string eventName)
        {
            return await Task.Run(() =>
                _eventStoreSqlContext.EventMetaData.Where(x => x.EventName == eventName).ToList());
        }

        public async Task<EventStore> AddEvent(EventStore eventStore)
        {
            return await Task.Run(() => _eventStoreSqlContext.EventMetaData.Add(eventStore).Entity);
        }
        
        public async Task Save()
        {
            await Task.Run(() => _eventStoreSqlContext.SaveChanges(true));
        }

        public bool Disposed;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    _eventStoreSqlContext.Dispose();
                }
            }
            this.Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
    }
}