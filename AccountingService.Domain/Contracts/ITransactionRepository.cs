using AccountingService.Domain.Models;

namespace AccountingService.Domain.Contracts
{

    public interface ITransactionRepository : IDisposable
    {

        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task<Transaction?> GetTransactionById(Guid id);
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountId(int accountId);
        Task<IEnumerable<Transaction>> GetCreditTransactionsByAccountId(int accountId);
        Task<IEnumerable<Transaction>> GetDebitTransactionsByAccountId(int accountId);
        Task Save();
    }
}