using System;

namespace ExpenseApp.Domain.Data.Models;

public class User : BaseEntity
{
    public required string Email { get; set; }

    public required string Password { get; set; }

}
