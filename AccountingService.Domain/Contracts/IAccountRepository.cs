using AccountingService.Domain.Models;

namespace AccountingService.Domain.Contracts
{

    public interface IAccountRepository : IDisposable
    {

        IEnumerable<Account> GetAllAccounts();
        Account GetAccountById(int id);
        Account AddAccount(Account account);
        Account DeactivateAccount(int id);
        void Save();
    }
}