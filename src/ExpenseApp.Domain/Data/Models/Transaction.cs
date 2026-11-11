using System;

namespace ExpenseApp.Domain.Data.Models;

public class Transaction : BaseEntity
{
    private Transaction() { }

    public TransactionType Type { get; private set; }

    public double Amount { get; private set; }

    public string Category { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public static Transaction Create(TransactionType type, double amount, string category, string description)
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

    public void Update(TransactionType type, double amount, string category, string description)
    {
        Type = type;
        Amount = amount;
        Category = category;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
