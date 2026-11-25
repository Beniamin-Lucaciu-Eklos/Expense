namespace ExpenseApp.Application.Transactions.TransactionQueries;

public sealed record TransactionTotalsResponse(
    double TotalIncome,
    double TotalExpenses,
    double NetBalance,
    int TransactionCount);
