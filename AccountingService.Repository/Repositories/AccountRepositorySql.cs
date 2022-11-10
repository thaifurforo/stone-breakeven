using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository.Repositories
{
    public class AccountRepositorySql : IAccountRepository, IDisposable
    {
        
        private readonly ContextSql _contextSql;

        public AccountRepositorySql(ContextSql contextSql)
        {
            _contextSql = contextSql ?? throw new ArgumentNullException(nameof(contextSql));
        }

        public async Task<Account> GetAccountById(int id)
        {
            return await Task.Run(() => _contextSql.Accounts.Find(id));
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await Task.Run(() => _contextSql.Accounts.ToList());
        }

        public async Task<Account> AddAccount(Account account)
        {

            return await Task.Run(() => _contextSql.Accounts.Add(account).Entity);
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            return await Task.Run(() => _contextSql.Accounts.Update(account).Entity);
        }

        public async Task<Account> DeactivateAccount(int id)
        {
            var account = _contextSql.Accounts.Find(id);
            account.DeactivateAccount();
            return await Task.Run(() => account);
        }

        public async Task Save()
        {
           await Task.Run(() => _contextSql.SaveChanges(true));
        }

        public bool disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _contextSql.Dispose();
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