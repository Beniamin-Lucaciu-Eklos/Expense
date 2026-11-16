using System;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApp.Infrastructure.Data;

public class UserRepository(ExpenseDbContext dbContext, ITokenService tokenService) : IUserRepository
{
    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        string jwtToken = tokenService.GenerateJwtTokenForUser(user);
        user.SetJwtToken(jwtToken);

        await dbContext.Users.AddAsync(user, cancellationToken);

        return user;
    }

    public Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AnyAsync(user => user.Email == email);
    }
}
