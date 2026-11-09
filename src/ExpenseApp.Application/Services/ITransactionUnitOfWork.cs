using System;
using ExpenseApp.Application.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Services;

public interface ITransactionUnitOfWork : IUnitOfWork
{
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<Transaction?> GetByIdAsync(int id);

    Task<Transaction> CreateAsync(Transaction transaction);

    Task<Transaction?> UpdateAsync(Transaction transaction);

    Task<Transaction?> Delete(int id);
}
