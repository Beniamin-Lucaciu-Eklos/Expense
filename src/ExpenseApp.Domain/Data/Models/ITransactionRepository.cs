using System;

namespace ExpenseApp.Domain.Data.Models;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllAsync(int userId);
    Task<TransactionPage> GetPageAsync(
        int userId,
        int skip,
        int take,
        CancellationToken cancellationToken);
    Task<Transaction?> GetByIdAsync(int id, int userId);
    Task<TransactionTotals> GetMonthlyTotalsAsync(
        int userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken);
    Task<TransactionTotals> GetTotalsAsync(int userId, CancellationToken cancellationToken);

    Task<Transaction> CreateAsync(Transaction transaction);

    Task<Transaction?> UpdateAsync(int id, Transaction transaction);

    Task<Transaction?> Delete(int id, int userId);
}
