using System;
using ExpenseApp.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApp.Infrastructure.Data;

public class TransactionRepository(ExpenseDbContext dbContext) : ITransactionRepository
{
    public async Task<IEnumerable<Transaction>> GetAllAsync(int userId)
    {
        return await dbContext.Transactions.Include(t => t.User)
                .Where(transaction => transaction.UserId == userId)
                .AsNoTracking().ToListAsync();
    }

    public async Task<TransactionPage> GetPageAsync(
        int userId,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var userTransactions = dbContext.Transactions
            .AsNoTracking()
            .Where(transaction => transaction.UserId == userId);
        var totalCount = await userTransactions.CountAsync(cancellationToken);
        var transactions = await userTransactions
            .OrderByDescending(transaction => transaction.CreatedAt)
            .ThenByDescending(transaction => transaction.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new TransactionPage(transactions, totalCount);
    }

    public async Task<TransactionTotals> GetMonthlyTotalsAsync(
        int userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var totals = await dbContext.Transactions
            .AsNoTracking()
            .Where(transaction =>
                transaction.UserId == userId &&
                transaction.CreatedAt >= startDate &&
                transaction.CreatedAt < endDate)
            .GroupBy(_ => 1)
            .Select(group => new TransactionTotals(
                group.Sum(transaction =>
                    transaction.Type == TransactionType.Income ? transaction.Amount : 0d),
                group.Sum(transaction =>
                    transaction.Type == TransactionType.Expense ? transaction.Amount : 0d),
                group.Count()))
            .SingleOrDefaultAsync(cancellationToken);

        return totals ?? TransactionTotals.Empty;
    }

    public async Task<TransactionTotals> GetTotalsAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var totals = await dbContext.Transactions
            .AsNoTracking()
            .Where(transaction => transaction.UserId == userId)
            .GroupBy(_ => 1)
            .Select(group => new TransactionTotals(
                group.Sum(transaction =>
                    transaction.Type == TransactionType.Income ? transaction.Amount : 0d),
                group.Sum(transaction =>
                    transaction.Type == TransactionType.Expense ? transaction.Amount : 0d),
                group.Count()))
            .SingleOrDefaultAsync(cancellationToken);

        return totals ?? TransactionTotals.Empty;
    }

    public async Task<Transaction?> GetByIdAsync(int id, int userId)
    {
        return await dbContext.Transactions.FindAsync(id, userId);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        dbContext.Transactions.Add(transaction);
        return transaction;
    }

    public async Task<Transaction?> UpdateAsync(int id, Transaction transaction)
    {
        if (id == 0)
            return null;

        var transactionFromDb = await GetByIdAsync(id, transaction.UserId);
        if (transactionFromDb is null)
            return null;

        transactionFromDb.Update(transaction.Type,
            transaction.Amount,
            transaction.Category,
            transaction.Description);

        dbContext.Transactions.Update(transactionFromDb);
        return transactionFromDb;
    }

    public async Task<Transaction?> Delete(int id, int userId)
    {
        var transaction = await GetByIdAsync(id, userId);
        if (transaction is null)
            return null;

        dbContext.Transactions.Remove(transaction);

        return transaction;
    }
}
