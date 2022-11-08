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

        public Account GetAccountById(int id)
        {
            return _contextInMemory.Accounts.Find(id);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _contextInMemory.Accounts.ToList();
        }

        public Account AddAccount(Account account)
        {

            return _contextInMemory.Accounts.Add(account).Entity;
        }

        public Account DeactivateAccount(int id)
        {
            var account = _contextInMemory.Accounts.Find(id);
            account.DeactivateAccount();
            return account;
        }

        public void Save()
        {
            _contextInMemory.SaveChanges(true);
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