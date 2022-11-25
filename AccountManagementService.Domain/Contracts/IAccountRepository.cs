using AccountManagementService.Domain.Models;

namespace AccountManagementService.Domain.Contracts
{

    public interface IAccountRepository : IDisposable
    {

        Task<IEnumerable<Account>> GetAllAccounts();
        Task<Account?> GetAccountById(int? accountId);
        Task<Account> AddAccount(Account account);
        Task<Account> UpdateAccount(Account account);
        Task<Account> DeactivateAccount(int id);
        Task Save(); 
    }
}