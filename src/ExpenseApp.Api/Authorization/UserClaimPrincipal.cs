using System;
using System.Security.Claims;
using ExpenseApp.Application.Authorization;

namespace ExpenseApp.Api.Authorization;

public class UserClaimPrincipal : IUserClaimPrincipal
{
    private readonly IHttpContextAccessor _accessor;

    public UserClaimPrincipal(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    private ClaimsPrincipal User => _accessor.HttpContext?.User
       ?? new ClaimsPrincipal(new ClaimsIdentity());

    public string? GetNameIdentifiedValue() => User.FindFirstValue(ClaimTypes.NameIdentifier);
}
