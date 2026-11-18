using System;

namespace ExpenseApp.Domain.Data.Models;

public class User : BaseEntity
{
    private User() { }

    public string Email { get; private set; } = default!;

    public string Password { get; private set; } = default!;

    public string Token { get; private set; } = default!;

    public void SetJwtToken(string jwtToken) => Token = jwtToken;

    public static User Create(
        string email,
        string password)
    {
        var user = new User()
        {
            Email = email,
            Password = password,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return user;
    }

}
