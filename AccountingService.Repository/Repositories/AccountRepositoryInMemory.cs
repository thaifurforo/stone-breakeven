using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository.Repositories
{
    public class AccountRepositoryInMemory : IAccountRepository, IDisposable
    {
        
        private readonly ContextInMemory _contextInMemory;

        public AccountRepositoryInMemory(ContextInMemory contextInMemory)
        {
            _contextInMemory = contextInMemory ?? throw new ArgumentNullException(nameof(contextInMemory));
        }

        public async Task<Account> GetAccountById(int id)
        {
            return await Task.Run(() => _contextInMemory.Accounts.Find(id));
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await Task.Run(() => _contextInMemory.Accounts.ToList());
        }

        public async Task<Account> AddAccount(Account account)
        {

            return await Task.Run(() => _contextInMemory.Accounts.Add(account).Entity);
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            return await Task.Run(() => _contextInMemory.Accounts.Update(account).Entity);
        }

        public async Task<Account> DeactivateAccount(int id)
        {
            var account = _contextInMemory.Accounts.Find(id);
            account.DeactivateAccount();
            return await Task.Run(() => account);
        }

        public async Task Save()
        {
            await Task.Run(() => _contextInMemory.SaveChanges(true));
        }

        public bool disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _contextInMemory.Dispose();
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