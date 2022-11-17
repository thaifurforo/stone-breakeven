using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Repository.Contexts;

namespace AccountingService.Repository.Repositories
{
    public class AccountInMemoryRepository : IAccountRepository, IDisposable
    {
        
        private readonly ReadModelInMemoryContext _readModelInMemoryContext;

        public AccountInMemoryRepository(ReadModelInMemoryContext readModelInMemoryContext)
        {
            _readModelInMemoryContext = readModelInMemoryContext ?? throw new ArgumentNullException(nameof(readModelInMemoryContext));
        }

        public async Task<Account?> GetAccountById(int? id)
        {
            return await Task.Run(() => _readModelInMemoryContext.Accounts.Find(id));
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await Task.Run(() => _readModelInMemoryContext.Accounts.ToList());
        }

        public async Task<Account> AddAccount(Account account)
        {

            return await Task.Run(() => _readModelInMemoryContext.Accounts.Add(account).Entity);
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            return await Task.Run(() => _readModelInMemoryContext.Accounts.Update(account).Entity);
        }

        public async Task<Account> DeactivateAccount(int id)
        {
            var account = _readModelInMemoryContext.Accounts.Find(id);
            account.DeactivateAccount();
            return await Task.Run(() => account);
        }

        public async Task Save()
        {
            await Task.Run(() => _readModelInMemoryContext.SaveChanges(true));
        }

        public async Task<Account> GetAccountById(int transactionAccountId)
        {
            return (await Task.Run(() => _readModelInMemoryContext.Accounts.Find(transactionAccountId)))!;
        }

        public bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _readModelInMemoryContext.Dispose();
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