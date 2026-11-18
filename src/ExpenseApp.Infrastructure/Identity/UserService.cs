using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;
using Humanizer;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseApp.Infrastructure.Identity;

public sealed class UserService(IUserClaimPrincipal userClaimPrincipal) : IUserService
{
    private const string UserIdClaim = ClaimTypes.NameIdentifier;

    public string GenerateJwtToken(User user)
    {
        var claims = new Claim[]
        {
            new Claim(UserIdClaim, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-secure-secret-key-32-chars-long"));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: "ben@ben.dot",
            audience: "ben@ben.dot",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Result<int> GetUserIdFromNameIdentifiedClaim()
    {
       string? nameIdentifierValue = userClaimPrincipal.GetNameIdentifiedValue();

       return int.TryParse(nameIdentifierValue, out int userId) ? Result.Success(userId) : Result.Failure<int>(Error.NullValue);
    }
}
