using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Models;
using AccountManagementService.Repository.Contexts;

namespace AccountManagementService.Repository.Repositories
{
    public class TransactionSqlRepository : ITransactionRepository, IDisposable
    {
        
        private readonly ReadModelSqlContext _readModelSqlContext;

        public TransactionSqlRepository(ReadModelSqlContext readModelSqlContext)
        {
            _readModelSqlContext = readModelSqlContext ?? throw new ArgumentNullException(nameof(readModelSqlContext));
        }

        public async Task<Transaction?> GetTransactionById(Guid id)
        {
            return await Task.Run(() => _readModelSqlContext.Transaction.Find(id));
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            return await Task.Run(() => _readModelSqlContext.Transaction.ToList());
        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {

            return await Task.Run(() => _readModelSqlContext.Transaction.Add(transaction).Entity);
        }
        
        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountId(int accountId)
        {
            return await Task.Run(() => _readModelSqlContext.Transaction.
                Where(x => x.CreditAccountId == accountId || x.DebitAccountId == accountId).ToList());
        }
        
        public async Task<IEnumerable<Transaction>> GetTransactionsByCreditAccountId(int accountId)
        {
            return await Task.Run(() => _readModelSqlContext.Transaction.
                Where(x => x.CreditAccountId == accountId).ToList());
        }
        
        public async Task<IEnumerable<Transaction>> GetTransactionsByDebitAccountId(int accountId)
        {
            return await Task.Run(() => _readModelSqlContext.Transaction.
                Where(x => x.DebitAccountId == accountId).ToList());
        }
        
        public async Task Save()
        {
           await Task.Run(() => _readModelSqlContext.SaveChanges(true));
        }

        public bool disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _readModelSqlContext.Dispose();
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