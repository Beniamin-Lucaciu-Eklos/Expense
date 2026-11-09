using System;

namespace ExpenseApp.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
