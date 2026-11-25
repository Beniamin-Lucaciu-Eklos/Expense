namespace ExpenseApp.Domain.Data.Models;

public sealed record TransactionTotals(
    double Income,
    double Expenses,
    int TransactionCount)
{
    public static TransactionTotals Empty { get; } = new(0, 0, 0);
}
