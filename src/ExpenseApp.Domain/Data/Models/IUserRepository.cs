using System;

namespace ExpenseApp.Domain.Data.Models;

public interface IUserRepository
{
    Task<User?> Load(string email, CancellationToken cancellationToken);
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, string? password = null, CancellationToken cancellationToken = default);
}
