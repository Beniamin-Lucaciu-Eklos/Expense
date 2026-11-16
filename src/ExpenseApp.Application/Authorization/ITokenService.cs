using System;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Authorization;

public interface ITokenService
{
    string GenerateJwtTokenForUser(User user);
}
