using System;

namespace ExpenseApp.Application.Authorization;

public interface IUserClaimPrincipal
{
    string? GetNameIdentifierValue();
}
