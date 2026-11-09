using System;
using ExpenseApp.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApp.Infrastructure.Data;

public class TransactionRepository(ExpenseDbContext dbContext) : ITransactionRepository
{
    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await dbContext.Transactions.AsNoTracking().ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await dbContext.Transactions.FindAsync(id);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        dbContext.Transactions.Add(transaction);
        return transaction;
    }

    public async Task<Transaction?> UpdateAsync(Transaction transaction)
    {
        if (transaction is null or { Id: <= 0 })
            return null;

        var transactionFromDb = await GetByIdAsync(transaction.Id);
        if (transactionFromDb is null)
            return null;

        transactionFromDb.Amount = transaction.Amount;
        transactionFromDb.Category = transaction.Category;
        transactionFromDb.Description = transaction.Description;
        transactionFromDb.UpdateAt = DateTime.UtcNow;

        dbContext.Transactions.Update(transactionFromDb);
        return transactionFromDb;
    }

    public async Task<Transaction?> Delete(int id)
    {
        var transaction = await GetByIdAsync(id);
        if (transaction is null)
            return null;

        dbContext.Transactions.Remove(transaction);

        return transaction;
    }
}
