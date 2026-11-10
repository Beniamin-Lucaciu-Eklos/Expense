using System;

namespace ExpenseApp.Domain.Data.Models;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<Transaction?> GetByIdAsync(int id);

    Task<Transaction> CreateAsync(Transaction transaction);

    Task<Transaction?> UpdateAsync(int id, Transaction transaction);

    Task<Transaction?> Delete(int id);
}
