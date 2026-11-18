using System;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApp.Infrastructure.Data;

public class UserRepository(ExpenseDbContext dbContext, IUserService userService) : IUserRepository
{
    public Task<User?> Load(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        string jwtToken = userService.GenerateJwtToken(user);
        user.SetJwtToken(jwtToken);

        await dbContext.Users.AddAsync(user, cancellationToken);

        return user;
    }

    public Task<bool> ExistsAsync(string email, string? password = null, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AnyAsync(user => user.Email == email
        && (!string.IsNullOrWhiteSpace(password)
        ? user.Password == password
        : 1 == 1));
    }
}
