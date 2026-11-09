using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed class TransactionResponse
{
    public int Id { get; init; }

    public required string Type { get; init; }

    public double Amount { get; init; }

    public required string Category { get; init; }

    public required string Description { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdateAt { get; init; }
}

internal static class TransactionMapping
{
    public static IReadOnlyList<TransactionResponse> AsResponse(this IEnumerable<Transaction> transactions)
    {
        return transactions.Select(AsResponse).ToList();
    }

    public static TransactionResponse AsResponse(this Transaction transaction)
    {
        return new TransactionResponse
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Category = transaction.Category,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt,
            UpdateAt = transaction.UpdateAt,
        };
    }
}
