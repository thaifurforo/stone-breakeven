using AccountingService.Domain.Models;

namespace AccountingService.Domain.Contracts
{

    public interface IAccountRepository : IDisposable
    {

        Task<IEnumerable<Account>> GetAllAccounts();
        Task<Account> GetAccountById(int id);
        Task<Account> AddAccount(Account account);
        Task<Account> UpdateAccount(Account account);
        Task<Account> DeactivateAccount(int id);
        Task Save();
    }
}