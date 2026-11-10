using System;

namespace ExpenseApp.Domain.Data.Models;

public class Transaction : BaseEntity
{
    private Transaction() { }

    public string Type { get; private set; } = default!;

    public double Amount { get; private set; }

    public string Category { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public static Transaction Create(string type, double amount, string category, string description)
    {
        var transaction = new Transaction()
        {
            Type = type,
            Amount = amount,
            Category = category,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return transaction;
    }

    public void Update(string type, double amount, string category, string description)
    {
        Type = type;
        Amount = amount;
        Category = category;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
