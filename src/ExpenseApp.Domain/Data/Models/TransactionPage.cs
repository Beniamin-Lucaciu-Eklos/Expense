namespace ExpenseApp.Domain.Data.Models;

public sealed record TransactionPage(
    IReadOnlyList<Transaction> Items,
    int TotalCount);
