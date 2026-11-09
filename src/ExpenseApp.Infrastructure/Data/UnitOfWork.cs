using System;
using ExpenseApp.Application.Abstractions;

namespace ExpenseApp.Infrastructure.Data;

public class UnitOfWork(ExpenseDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }
}
