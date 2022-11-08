using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository.Repositories
{
    public class AccountRepository : IAccountRepository, IDisposable
    {
        
        private readonly ReadModelContext _context;

        public AccountRepository(ReadModelContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Account GetAccountById(int id)
        {
            return _context.Accounts.Find(id);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public Account AddAccount(Account account)
        {

            return _context.Accounts.Add(account).Entity;
        }

        public Account DeactivateAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            account.DeactivateAccount();
            return account;
        }

        public void Save()
        {
            _context.SaveChanges(true);
        }

        public bool disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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