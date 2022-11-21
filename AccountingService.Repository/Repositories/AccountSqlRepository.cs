using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Repository.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository.Repositories
{
    public class AccountSqlRepository : IAccountRepository, IDisposable
    {
        
        private readonly ReadModelSqlContext _readModelSqlContext;

        public AccountSqlRepository(ReadModelSqlContext readModelSqlContext)
        {
            _readModelSqlContext = readModelSqlContext ?? throw new ArgumentNullException(nameof(readModelSqlContext));
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            var accounts = _readModelSqlContext.Account
                .Include(x => x.Transactions);
            return await Task.Run(() => accounts.ToList());
        }

        public async Task<Account> AddAccount(Account account)
        {

            return await Task.Run(() => _readModelSqlContext.Account.Add(account).Entity);
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            return await Task.Run(() => _readModelSqlContext.Account.Update(account).Entity);
        }

        public async Task<Account> DeactivateAccount(int id)
        {
            var account = await _readModelSqlContext.Account.FindAsync(id);
            account.DeactivateAccount();
            return await Task.Run(() => account);
        }

        public async Task Save()
        {
           await Task.Run(() => _readModelSqlContext.SaveChanges(true));
        }

        public async Task<Account?> GetAccountById(int? accountId)
        {
            var account = _readModelSqlContext.Account
                .Where(x => x.Id == accountId)
                .Include(x => x.Transactions);
            return await await Task.FromResult(account.FirstOrDefaultAsync())!;        
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