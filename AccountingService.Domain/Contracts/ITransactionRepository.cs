using AccountingService.Domain.Models;

namespace AccountingService.Domain.Contracts
{

    public interface ITransactionRepository : IDisposable
    {

        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task<Transaction?> GetTransactionById(Guid id);
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId);
        Task<IEnumerable<Transaction>> GetCreditTransactionsByAccount(int accountId);
        Task<IEnumerable<Transaction>> GetDebitTransactionsByAccount(int accountId);
        Task Save();
    }
}