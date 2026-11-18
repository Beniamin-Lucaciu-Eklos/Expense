using System;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Authorization;

public interface IUserService
{
    string GenerateJwtToken(User user);

    Result<int> GetUserIdFromNameIdentifiedClaim();
}
