using AccountManagementService.Domain.Models;

namespace AccountManagementService.Domain.Contracts
{

    public interface ITransactionRepository : IDisposable
    {

        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task<Transaction?> GetTransactionById(Guid id);
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountId(int accountId);
        Task<IEnumerable<Transaction>> GetTransactionsByCreditAccountId(int accountId);
        Task<IEnumerable<Transaction>> GetTransactionsByDebitAccountId(int accountId);
        Task Save();
    }
}