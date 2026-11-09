using System;
using ExpenseApp.Application.Services;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Infrastructure.Data;

public class TransactionUnitOfWork(ITransactionRepository transactionRepository, ExpenseDbContext dbContext)
    : UnitOfWork(dbContext), ITransactionUnitOfWork
{
    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await transactionRepository.GetAllAsync();
    }

    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await transactionRepository.GetByIdAsync(id);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        var entry = await transactionRepository.CreateAsync(transaction);
        await SaveChangesAsync();
        return entry;
    }

    public async Task<Transaction?> UpdateAsync(Transaction transaction)
    {
        var entry = await transactionRepository.UpdateAsync(transaction);
        if (entry is null)
            return null;

        await SaveChangesAsync();
        return entry;
    }

    public async Task<Transaction?> Delete(int id)
    {
        var entry = await transactionRepository.Delete(id);
        if (entry is null)
            return null;

        await SaveChangesAsync();
        return entry;
    }
}
